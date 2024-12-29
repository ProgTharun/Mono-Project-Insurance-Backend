using AutoMapper;
using InsurancePolicy.DTOs;
using InsurancePolicy.Helpers;
using InsurancePolicy.Models;
using InsurancePolicy.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace InsurancePolicy.Services
{
    public class CustomerQueryService : ICustomerQueryService
    {
        private readonly IRepository<CustomerQuery> _queryRepository;
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IMapper _mapper;

        public CustomerQueryService(IRepository<CustomerQuery> queryRepository, IRepository<Employee> employeeRepository, IMapper mapper)
        {
            _queryRepository = queryRepository;
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public Guid AddQuery(CustomerQueryRequestDto queryDto)
        {
            var query = _mapper.Map<CustomerQuery>(queryDto);
            _queryRepository.Add(query);
            Log.Information("New record added:" + query.QueryId);
            return query.QueryId;
        }
        public int GetComplaintCountByCustomerId(Guid customerId)
        {
            var count=_queryRepository.GetAll().Where(c=>c.CustomerId == customerId).Count();
            return count;
        }
        public int GetQueryCount()
        {
            return _queryRepository.GetAll().Count();
        }
        public void UpdateQuery(CustomerQueryRequestDto queryDto)
        {
            var existingQuery = _queryRepository.GetById(queryDto.QueryId.Value);
            if (existingQuery == null)
                throw new KeyNotFoundException("Query not found.");

            _mapper.Map(queryDto, existingQuery);
            Log.Information("Record upadted", existingQuery.QueryId);// Log the count of records retrieved

            _queryRepository.Update(existingQuery);
        }

        public void ResolveQuery(Guid queryId, string response, Guid employeeId)
        {
            var query = _queryRepository.GetAll()
                .Include(q => q.ResolvedBy)
                .FirstOrDefault(q => q.QueryId == queryId);

            if (query == null)
                throw new KeyNotFoundException("Query not found.");

            if (query.IsResolved)
                throw new InvalidOperationException("Query is already resolved.");

            var employee = _employeeRepository.GetById(employeeId);
            if (employee == null)
                throw new KeyNotFoundException("Employee not found.");

            query.Response = response;
            query.IsResolved = true;
            query.ResolvedByEmployeeId = employeeId;
            query.ResolvedAt = DateTime.Now;

            _queryRepository.Update(query);
            Log.Information("Record upadted", query.QueryId);// Log the count of records retrieved

        }

        public CustomerQueryResponseDto GetQueryById(Guid queryId)
        {
            var query = _queryRepository.GetAll()
                .Include(q => q.Customer)
                .Include(q => q.ResolvedBy)
                .FirstOrDefault(q => q.QueryId == queryId);

            if (query == null)
                throw new KeyNotFoundException("Query not found.");
            Log.Information("Record Retrieved.",query.QueryId);
            return _mapper.Map<CustomerQueryResponseDto>(query);
        }

        public PageList<CustomerQueryResponseDto> GetAllQueriesByCustomer(Guid customerId, PageParameters pageParameters)
        {
            var queries = _queryRepository.GetAll()
                .Where(q => q.CustomerId == customerId)
                .Include(q => q.ResolvedBy)
                .Include(c=>c.Customer)
                .ToList();
                var paginatedQueries = PageList<CustomerQueryResponseDto>.ToPagedList(
                    _mapper.Map<List<CustomerQueryResponseDto>>(queries),
                    pageParameters.PageNumber,
                    pageParameters.PageSize);
            Log.Information("Records Retrieved. Count: {count}", queries.Count);// Log the count of records retrieved

            return paginatedQueries;
            
        }

        public List<CustomerQueryResponseDto> GetAllQueries()
        {
            var queries = _queryRepository.GetAll()
                .Include(q => q.Customer)
                .Include(q => q.ResolvedBy)
                .ToList();
            Log.Information("Records Retrieved. Count: {count}", queries.Count);// Log the count of records retrieved

            return _mapper.Map<List<CustomerQueryResponseDto>>(queries);
        }

        public PageList<CustomerQueryResponseDto> GetPaginatedQueries(PageParameters pageParameters)
        {
            var queries = _queryRepository.GetAll()
                .Include(q => q.Customer)
                .Include(q => q.ResolvedBy)
                .ToList();

            var paginatedQueries = PageList<CustomerQueryResponseDto>.ToPagedList(
                _mapper.Map<List<CustomerQueryResponseDto>>(queries),
                pageParameters.PageNumber,
                pageParameters.PageSize
            );
            Log.Information("Records Retrieved. Count: {count}", queries.Count);// Log the count of records retrieved

            return paginatedQueries;
        }
    }
}
