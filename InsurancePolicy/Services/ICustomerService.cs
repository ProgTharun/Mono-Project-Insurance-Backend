using InsurancePolicy.DTOs;
using InsurancePolicy.Helpers;
using InsurancePolicy.Models;

namespace InsurancePolicy.Services
{
    public interface ICustomerService
    {
        public List<CustomerResponseDto> GetAll();
        PageList<CustomerResponseDto> GetAllPaginated(PageParameters pageParameters);
        public List<CustomerResponseDto> GetCustomersByAgentId(Guid agentId);
        public CustomerResponseDto GetById(Guid id);
        public Guid Add(CustomerRequestDto customer);
        public bool Update(CustomerRequestDto customer);
        public bool Delete(Guid id);
        public bool UpdateUsername(CustomerRequestDto customerRequestDto);
        public PageList<CustomerResponseDto> GetCustomersByAgentIdPaginated(Guid agentId, PageParameters pageParameters);
        public int GetCustomersCount();


    }
}
