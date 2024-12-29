using InsurancePolicy.DTOs;

namespace InsurancePolicy.Services
{
    public interface IInsuranceSettingsService
    {
        Guid Add(InsuranceSettingsRequestDto requestDto);
        InsuranceSettingsResponseDto GetById(Guid id);
        List<InsuranceSettingsResponseDto> GetAll();
    }
}
