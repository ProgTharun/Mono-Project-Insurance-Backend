using InsurancePolicy.DTOs;
using InsurancePolicy.Helpers;
using InsurancePolicy.Models;

namespace InsurancePolicy.Services
{
    public interface IInsurancePlanService
    {
        List<InsurancePlanResponseDto> GetAll();
        PageList<InsurancePlanResponseDto> GetAllPaginated(PageParameters pageParameters);
        InsurancePlanResponseDto GetById(Guid id);
        Guid Add(InsurancePlanRequestDto plan);
        void Update(InsurancePlanRequestDto plan);
        void Delete(Guid id);
        void Activate(Guid id);
        public int GetPlanCount();
    }
}
