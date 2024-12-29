using InsurancePolicy.DTOs;
using InsurancePolicy.Helpers;

namespace InsurancePolicy.Services
{
    public interface IWithdrawalRequestService
    {
        Guid CreateRequest(WithdrawalRequestDto requestDto);
        void ApproveRequest(Guid requestId);
        void RejectRequest(Guid requestId);
        WithdrawalRequestResponseDto GetRequestById(Guid requestId);
        List<WithdrawalRequestResponseDto> GetAllRequests();
        public double GetTotalCommission(Guid agentId);
        public PageList<WithdrawalRequestResponseDto> GetRequestsByAgentIdPaginated(Guid agentId, PageParameters pageParameters);
        public int GetCount();


    }
}
