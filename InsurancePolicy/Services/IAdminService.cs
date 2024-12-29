using InsurancePolicy.DTOs;
using InsurancePolicy.Models;

namespace InsurancePolicy.Services
{
    public interface IAdminService
    {
        public List<AdminResponseDto> GetAll();
        public AdminResponseDto GetByName(string name);
        public AdminResponseDto GetById(Guid id);
        public Guid Add(AdminRequestDto admin);
        public bool Update(AdminRequestDto admin);
        public bool UpdateUsername(AdminRequestDto adminRequestDto);


    }
}
