using AutoMapper;
using InsurancePolicy.DTOs;
using InsurancePolicy.Helpers;
using InsurancePolicy.Models;
using InsurancePolicy.Repositories;
using Serilog;

namespace InsurancePolicy.Services
{
    public class CommissionService : ICommissionService
    {
        private readonly IRepository<Commission> _commissionRepository;
        private readonly IRepository<Policy> _policyRepository;
        private readonly IRepository<Agent> _agentRepository;
        private readonly IMapper _mapper;

        public CommissionService(
            IRepository<Commission> commissionRepository,
            IRepository<Policy> policyRepository,
            IRepository<Agent> agentRepository,
            IMapper mapper)
        {
            _commissionRepository = commissionRepository;
            _policyRepository = policyRepository;
            _agentRepository = agentRepository;
            _mapper = mapper;
        }

        public Guid AddCommission(CommissionRequestDto commissionDto)
        {
            // Validate agent existence
            var agent = _agentRepository.GetById(commissionDto.AgentId);
            if (agent == null)
                throw new ArgumentException("Invalid Agent ID.");

            // Validate policy linkage if provided
            if (commissionDto.PolicyNo.HasValue)
            {
                var policy = _policyRepository.GetById(commissionDto.PolicyNo.Value);
                if (policy == null)
                    throw new ArgumentException("Invalid Policy ID.");
            }

            // Map and save commission
            var commission = _mapper.Map<Commission>(commissionDto);
            _commissionRepository.Add(commission);
            Log.Information("New record added:" + commission.CommissionId);

            return commission.CommissionId;
        }
        public PageList<CommissionResponseDto> GetAllCommissionsPaginated(PageParameters pageParameters)
        {
            var commissions = _commissionRepository.GetAll().ToList();

            var pagedCommissions = PageList<CommissionResponseDto>.ToPagedList(
                _mapper.Map<List<CommissionResponseDto>>(commissions),
                pageParameters.PageNumber,
                pageParameters.PageSize
            );
            Log.Information("Records Retrieved. Count: {count}", commissions.Count);// Log the count of records retrieved

            return pagedCommissions;
        }

        public PageList<CommissionResponseDto> GetCommissionsByAgentPaginated(Guid agentId, PageParameters pageParameters)
        {
            var commissions = _commissionRepository.GetAll()
                .Where(c => c.AgentId == agentId)
                .ToList();

            var pagedCommissions = PageList<CommissionResponseDto>.ToPagedList(
                _mapper.Map<List<CommissionResponseDto>>(commissions),
                pageParameters.PageNumber,
                pageParameters.PageSize
            );
            Log.Information("Records Retrieved. Count: {count}", commissions.Count);// Log the count of records retrieved

            return pagedCommissions;
        }

        public int GetCount()
        {
            return _commissionRepository.GetAll().Count();
        }
        public List<CommissionResponseDto> GetCommissionsByAgent(Guid agentId)
        {
            var commissions = _commissionRepository.GetAll()
                .Where(c => c.AgentId == agentId)
                .ToList();
            Log.Information("Records Retrieved. Count: {count}", commissions.Count);// Log the count of records retrieved

            return _mapper.Map<List<CommissionResponseDto>>(commissions);
        }

        public List<CommissionResponseDto> GetCommissionsByPolicy(Guid policyId)
        {
            var commissions = _commissionRepository.GetAll()
                .Where(c => c.PolicyNo == policyId)
                .ToList();
            Log.Information("Records Retrieved. Count: {count}", commissions.Count);// Log the count of records retrieved

            return _mapper.Map<List<CommissionResponseDto>>(commissions);
        }

        public List<CommissionResponseDto> GetAllCommissions()
        {
            var commissions = _commissionRepository.GetAll().ToList();
            Log.Information("Records Retrieved. Count: {count}", commissions.Count);// Log the count of records retrieved

            return _mapper.Map<List<CommissionResponseDto>>(commissions);
        }
    }
}
