using InsurancePolicy.DTOs;
using InsurancePolicy.Helpers;

namespace InsurancePolicy.Services
{
    public interface IInstallmentService
    {
        Guid AddInstallment(InstallmentRequestDto installmentDto);
        bool UpdateInstallment(InstallmentRequestDto installmentDto);
        InstallmentResponseDto GetInstallmentById(Guid installmentId);
        List<InstallmentResponseDto> GetAllInstallmentsForPolicy(Guid policyId);
        PageList<InstallmentResponseDto> GetPaginatedInstallmentsForPolicy(Guid policyId, PageParameters pageParameters);
        bool DeleteInstallment(Guid installmentId);
    }
}
