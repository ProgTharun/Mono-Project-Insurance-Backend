using AutoMapper;
using InsurancePolicy.DTOs;
using InsurancePolicy.enums;
using InsurancePolicy.Helpers;
using InsurancePolicy.Models;
using InsurancePolicy.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace InsurancePolicy.Services
{
    public class ClaimService : IClaimService
    {
        private readonly IRepository<Claim> _claimRepository;
        private readonly IMapper _mapper;

        public ClaimService(IRepository<Claim> claimRepository, IMapper mapper)
        {
            _claimRepository = claimRepository;
            _mapper = mapper;
        }
        public int GetClaimCountByCUstomerId(Guid customerId)
        {
            var count =_claimRepository.GetAll().Where(c=>c.CustomerId == customerId).Count();
            Log.Information("Records Retrieved. Count: {count}", count);// Log the count of records retrieved
            return count;
        }
        public Guid AddClaim(ClaimRequestDto requestDto)
        {
            var claim = _mapper.Map<Claim>(requestDto);
            claim.Status = Status.PENDING; // Set default status to PENDING

            _claimRepository.Add(claim); Log.Information("New record added:" + claim.ClaimId);

            return claim.ClaimId;
        }

        public void UpdateClaim(ClaimRequestDto requestDto)
        {
            var existingClaim = _claimRepository.GetById(requestDto.ClaimId.Value);
            if (existingClaim == null)
                throw new KeyNotFoundException("Claim not found.");

            _mapper.Map(requestDto, existingClaim);
            _claimRepository.Update(existingClaim);
            Log.Information("Record upadted", existingClaim.ClaimId);// Log the count of records retrieved

        }
        public PageList<ClaimResponseDto> GetClaimsByCustomerId(Guid customerId, PageParameters pageParameters)
        {
            var claims = _claimRepository.GetAll()
                .Include(c => c.Policy)
                .Include(c => c.Customer)
                .Where(c => c.CustomerId == customerId) // Filter by customerId
                .ToList();

            if (!claims.Any())
                throw new KeyNotFoundException("No claims found for the specified customer.");

            var paginatedClaims = PageList<ClaimResponseDto>.ToPagedList(
                _mapper.Map<List<ClaimResponseDto>>(claims),
                pageParameters.PageNumber,
                pageParameters.PageSize
            );
            Log.Information("Records Retrieved. Count: {count}", claims.Count);// Log the count of records retrieved

            return paginatedClaims;
        }
        public int TotalClaims()
        {
            return _claimRepository.GetAll().Count();
        }
        public void ApproveClaim(Guid claimId)
        {
            var claim = _claimRepository.GetById(claimId);
            if (claim == null)
                throw new KeyNotFoundException("Claim not found.");

            claim.Status = Status.APPROVED;
            claim.ApprovalDate = DateTime.Now;
            _claimRepository.Update(claim);
            Log.Information("Record upadted", claimId);// Log the count of records retrieved
        }

        public void RejectClaim(Guid claimId, string rejectionReason)
        {
            var claim = _claimRepository.GetById(claimId);
            if (claim == null)
                throw new KeyNotFoundException("Claim not found.");

            claim.Status = Status.REJECTED;
            claim.ClaimReason = rejectionReason;
            claim.RejectionDate = DateTime.Now;
            _claimRepository.Update(claim);
            Log.Information("Record Updated:" + claim.ClaimId);

        }

        public ClaimResponseDto GetClaimById(Guid claimId)
        {
            var claim = _claimRepository.GetAll()
                .Include(c => c.Policy)
                .Include(c => c.Customer)
                .FirstOrDefault(c => c.ClaimId == claimId);

            if (claim == null)
                throw new KeyNotFoundException("Claim not found.");
            Log.Information("Record retrived:" + claim.ClaimId);

            return _mapper.Map<ClaimResponseDto>(claim);
        }

        public List<ClaimResponseDto> GetAllClaims()
        {
            var claims = _claimRepository.GetAll()
                .Include(c => c.Policy)
                .Include(c => c.Customer)
                .ToList();
            Log.Information("Records Retrieved. Count: {count}", claims.Count);// Log the count of records retrieved

            return _mapper.Map<List<ClaimResponseDto>>(claims);
        }
        public PageList<ClaimResponseDto> GetAllPaginated(PageParameters pageParameters)
        {
            var claims = _claimRepository.GetAll()
                .Include(c => c.Policy)
                .Include(c => c.Customer)
                .ToList();

            var paginatedClaims = PageList<ClaimResponseDto>.ToPagedList(
                _mapper.Map<List<ClaimResponseDto>>(claims),
                pageParameters.PageNumber,
                pageParameters.PageSize
            );
            Log.Information("Records Retrieved. Count: {count}", claims.Count);// Log the count of records retrieved

            return paginatedClaims;
        }
    }
}
