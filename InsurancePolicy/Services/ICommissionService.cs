using InsurancePolicy.DTOs;
using InsurancePolicy.Helpers;

namespace InsurancePolicy.Services
{
    public interface ICommissionService
    {
        Guid AddCommission(CommissionRequestDto commissionDto);
        List<CommissionResponseDto> GetCommissionsByAgent(Guid agentId);
        List<CommissionResponseDto> GetCommissionsByPolicy(Guid policyId);
        List<CommissionResponseDto> GetAllCommissions();
        public PageList<CommissionResponseDto> GetCommissionsByAgentPaginated(Guid agentId, PageParameters pageParameters);
        public PageList<CommissionResponseDto> GetAllCommissionsPaginated(PageParameters pageParameters);
        public int GetCount();
    }
}
