using InsurancePolicy.DTOs;

namespace InsurancePolicy.Services
{
    public interface ITaxSettingsService
    {
        Guid Add(TaxSettingsRequestDto requestDto);
        void Update(TaxSettingsRequestDto requestDto);
        TaxSettingsResponseDto GetById(Guid id);
        List<TaxSettingsResponseDto> GetAll();
    }
}
