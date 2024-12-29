using InsurancePolicy.DTOs;
using InsurancePolicy.Helpers;

namespace InsurancePolicy.Services
{
    public interface IClaimService
    {
        Guid AddClaim(ClaimRequestDto requestDto);
        PageList<ClaimResponseDto> GetAllPaginated(PageParameters pageParameters);
        PageList<ClaimResponseDto> GetClaimsByCustomerId(Guid customerId, PageParameters pageParameters);
        void UpdateClaim(ClaimRequestDto requestDto);
        void ApproveClaim(Guid claimId);
        void RejectClaim(Guid claimId, string rejectionReason);
        ClaimResponseDto GetClaimById(Guid claimId);
        List<ClaimResponseDto> GetAllClaims();
        public int TotalClaims();
        public int GetClaimCountByCUstomerId(Guid customerId);
    }
}
