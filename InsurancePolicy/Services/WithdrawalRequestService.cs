using AutoMapper;
using InsurancePolicy.DTOs;
using InsurancePolicy.enums;
using InsurancePolicy.Helpers;
using InsurancePolicy.Models;
using InsurancePolicy.Repositories;
using InsurancePolicy.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Stripe;

public class WithdrawalRequestService : IWithdrawalRequestService
{
    private readonly IRepository<WithdrawalRequest> _withdrawalRequestRepository;
    private readonly IRepository<Commission> _commissionRepository;
    private readonly IMapper _mapper;

    public WithdrawalRequestService(
        IRepository<WithdrawalRequest> withdrawalRequestRepository,
        IRepository<Commission> commissionRepository,
        IMapper mapper)
    {
        _withdrawalRequestRepository = withdrawalRequestRepository;
        _commissionRepository = commissionRepository;
        _mapper = mapper;
    }

    public double GetTotalCommission(Guid agentId)
    {
        // Calculate the total commission for the given agent
        return _commissionRepository.GetAll()
            .Where(c => c.AgentId == agentId)
            .Sum(c => c.Amount);
    }
    public int GetCount()
    {
        return _withdrawalRequestRepository.GetAll().Count();
    }
    public Guid CreateRequest(WithdrawalRequestDto requestDto)
    {
        // Calculate total commission for the agent
        var totalCommission = GetTotalCommission(requestDto.AgentId.Value);

        if (requestDto.Amount > totalCommission)
            throw new InvalidOperationException("Requested amount exceeds available commission balance.");

        // Map DTO to entity
        var withdrawalRequest = _mapper.Map<WithdrawalRequest>(requestDto);

        // Assign calculated total commission and set status
        withdrawalRequest.TotalCommission = totalCommission;
        withdrawalRequest.Status = WithdrawalRequestStatus.PENDING;

        // Save the request
        _withdrawalRequestRepository.Add(withdrawalRequest);
        Log.Information("New record added:" +withdrawalRequest.WithdrawalRequestId);
        return withdrawalRequest.WithdrawalRequestId;
    }

    public void ApproveRequest(Guid requestId)
    {
        var request = _withdrawalRequestRepository.GetById(requestId);
        if (request == null)
            throw new KeyNotFoundException("Withdrawal request not found.");

        if (request.Status != WithdrawalRequestStatus.PENDING)
            throw new InvalidOperationException("Only pending requests can be approved.");

        var totalCommission = GetTotalCommission(request.AgentId.Value);

        if (request.Amount > totalCommission)
            throw new InvalidOperationException("Insufficient commission balance.");

        var commissions = _commissionRepository.GetAll()
            .Where(c => c.AgentId == request.AgentId)
            .OrderBy(c => c.IssueDate)
            .ToList();

        double remainingAmount = request.Amount;
        foreach (var commission in commissions)
        {
            if (remainingAmount <= 0) break;

            if (commission.Amount >= remainingAmount)
            {
                commission.Amount -= remainingAmount;
                _commissionRepository.Update(commission);
                break;
            }
            else
            {
                remainingAmount -= commission.Amount;
                commission.Amount = 0;
                _commissionRepository.Update(commission);
            }
        }

        request.Status = WithdrawalRequestStatus.APPROVED;
        request.ApprovedAt = DateTime.Now;
        Log.Information("Record upadted", requestId);// Log the count of records retrieved

        _withdrawalRequestRepository.Update(request);
    }

    public void RejectRequest(Guid requestId)
    {
        var request = _withdrawalRequestRepository.GetById(requestId);
        if (request == null)
            throw new KeyNotFoundException("Withdrawal request not found.");

        if (request.Status != WithdrawalRequestStatus.PENDING)
            throw new InvalidOperationException("Only pending requests can be rejected.");

        request.Status = WithdrawalRequestStatus.REJECTED;
        Log.Information("Record upadted", requestId);// Log the count of records retrieved

        _withdrawalRequestRepository.Update(request);
    }

    public WithdrawalRequestResponseDto GetRequestById(Guid requestId)
    {
        var request = _withdrawalRequestRepository.GetAll()
            .Include(w => w.Agent)
            .Include(w => w.Customer)
            .FirstOrDefault(w => w.WithdrawalRequestId == requestId);

        if (request == null)
            throw new KeyNotFoundException("Withdrawal request not found.");
        Log.Information("Record Retrieved. ", requestId);

        return _mapper.Map<WithdrawalRequestResponseDto>(request);
    }
    public PageList<WithdrawalRequestResponseDto> GetRequestsByAgentIdPaginated(Guid agentId, PageParameters pageParameters)
    {
        // Fetch all requests for the agent
        var requests = _withdrawalRequestRepository.GetAll()
            .Where(r => r.AgentId == agentId).Include(wr => wr.Agent) // Load Agent
        .ThenInclude(agent => agent.Customers) // Load Customers of the Agent
            .ThenInclude(customer => customer.Policies) // Load Policies of each Customer
    .ToList();

        

        // Map and paginate the requests
        var pagedRequests = PageList<WithdrawalRequestResponseDto>.ToPagedList(
            _mapper.Map<List<WithdrawalRequestResponseDto>>(requests),
            pageParameters.PageNumber,
            pageParameters.PageSize
        );

        return pagedRequests;
    }



    public List<WithdrawalRequestResponseDto> GetAllRequests()
    {
        var requests = _withdrawalRequestRepository.GetAll()
            .Include(wr => wr.Agent) // Load Agent
        .ThenInclude(agent => agent.Customers) // Load Customers of the Agent
            .ThenInclude(customer => customer.Policies)
            .ToList();
        Log.Information("Records Retrieved. Count{count}", requests.Count);

        return _mapper.Map<List<WithdrawalRequestResponseDto>>(requests);
    }
}
