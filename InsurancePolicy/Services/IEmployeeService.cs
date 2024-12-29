using InsurancePolicy.DTOs;
using InsurancePolicy.Helpers;
using InsurancePolicy.Models;

namespace InsurancePolicy.Services
{
    public interface IEmployeeService
    {
        public List<EmployeeResponseDto> GetAll();
        public EmployeeResponseDto GetById(Guid id);
        public Guid Add(EmployeeRequestDto employee);
        public bool Update(EmployeeRequestDto employee);
        public bool Delete(Guid id);
        public void Activate(Guid id);
        public void UpdateSalary(Guid id,double salary);
        PageList<EmployeeResponseDto> GetAllPaginated(PageParameters pageParameters);
        public int GetTotalCount();
        public bool UpdateUsername(EmployeeRequestDto employeeRequestDto);
    }
}
