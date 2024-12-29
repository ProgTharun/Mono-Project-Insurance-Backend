using InsurancePolicy.DTOs;
using InsurancePolicy.Helpers;

namespace InsurancePolicy.Services
{
    public interface ICustomerQueryService
    {
        Guid AddQuery(CustomerQueryRequestDto queryDto);
        void UpdateQuery(CustomerQueryRequestDto queryDto);
        void ResolveQuery(Guid queryId, string response, Guid employeeId);
        CustomerQueryResponseDto GetQueryById(Guid queryId);
        PageList<CustomerQueryResponseDto> GetAllQueriesByCustomer(Guid customerId,PageParameters pageParameters);
        List<CustomerQueryResponseDto> GetAllQueries();
        PageList<CustomerQueryResponseDto> GetPaginatedQueries(PageParameters pageParameters);
        public int GetQueryCount();
        public int GetComplaintCountByCustomerId(Guid customerId);


    }
}
