using AutoMapper;
using InsurancePolicy.Data;
using InsurancePolicy.DTOs;
using InsurancePolicy.Exceptions.AgentExceptions;
using InsurancePolicy.Exceptions.CustomerExceptions;
using InsurancePolicy.Helpers;
using InsurancePolicy.Models;
using InsurancePolicy.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.ComponentModel.DataAnnotations;

namespace InsurancePolicy.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IRepository<Customer> _repository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public CustomerService(IRepository<Customer> repository, IRepository<Role> roleRepository, IMapper mapper,AppDbContext context)
        {
            _repository = repository;
            _roleRepository = roleRepository;
            _mapper = mapper;
            _context = context;
        }

        public Guid Add(CustomerRequestDto customerRequestDto)
        {
            var errors = new List<string>();

            // Ensure the "Customer" role exists
            var customerRole = _roleRepository.GetAll().FirstOrDefault(r => r.Name == "Customer");
            if (customerRole == null)
                errors.Add("Customer role not found.");

            // Check if username already exists
            var existingUserByUsername = _repository.GetAll().FirstOrDefault(u => u.User.UserName == customerRequestDto.UserName);
            if (existingUserByUsername != null)
                errors.Add("Username already exists.");

            // Check if email already exists in the Customers table
            var existingUserByEmail = _repository.GetAll().FirstOrDefault(u => u.Email == customerRequestDto.Email);
            if (existingUserByEmail != null)
                errors.Add("Email already exists.");

            // If there are any validation errors, throw a ValidationException
            if (errors.Any())
                throw new ValidationException(string.Join("; ", errors));

            // Hash the password
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(customerRequestDto.Password);

            // Map CustomerRequestDto to Customer model
            var customer = _mapper.Map<Customer>(customerRequestDto);

            // Assign RoleId and hashed password to User
            customer.User.Password = hashedPassword;
            customer.User.RoleId = customerRole.Id;

            // Handle Address, City, and State
            // Fetch existing city and state from the database
            var existingState = _context.States.FirstOrDefault(s => s.StateName == customerRequestDto.State);
            var existingCity = _context.Cities.FirstOrDefault(c => c.CityName == customerRequestDto.City && c.State.StateName == customerRequestDto.State);

            if (customer.Address == null)
            {
                customer.Address = new Address
                {
                    HouseNo = customerRequestDto.HouseNo,
                    Apartment = customerRequestDto.Apartment,
                    Pincode = customerRequestDto.Pincode,
                    City = existingCity ?? new City
                    {
                        CityName = customerRequestDto.City,
                        State = existingState ?? new State
                        {
                            StateName = customerRequestDto.State
                        }
                    }
                };
            }

            _repository.Add(customer);
            Log.Information("New record added:" + customer.CustomerId);
            return customer.CustomerId;
        }
        public int GetCustomersCount()
        { 
            return _repository.GetAll().Count();
        }
        public List<CustomerResponseDto> GetCustomersByAgentId(Guid agentId)
        {
            // Retrieve customers associated with the given AgentId
            var customers = _repository.GetAll()
                .Include(c => c.User)
                .Include(a => a.Address)
                    .ThenInclude(ad => ad.City)
                        .ThenInclude(city => city.State)
                .Include(c => c.Policies)
                .Where(c => c.AgentId == agentId)
                .ToList();

            // Check if any customers are found
            if (!customers.Any())
                throw new CustomerNotFoundException("No customers found for the given AgentId.");

            // Map the customers to CustomerResponseDto and return
            Log.Information("Records Retrieved. Count: {count}", customers.Count);// Log the count of records retrieved

            return _mapper.Map<List<CustomerResponseDto>>(customers);
        }

        public CustomerResponseDto GetById(Guid id)
        {
            var customer = _repository.GetAll()
                .Include(c => c.User)
                .Include(c => c.Agent)
                .Include(c => c.Policies)
                .Include(a => a.Address)
                    .ThenInclude(ad => ad.City)
                        .ThenInclude(city => city.State)
                .FirstOrDefault(c => c.CustomerId == id);

            if (customer == null)
                throw new CustomerNotFoundException("No such customer found.");
            Log.Information("Record Retrieved. ",customer.CustomerId);
            return _mapper.Map<CustomerResponseDto>(customer);
        }
        public PageList<CustomerResponseDto> GetCustomersByAgentIdPaginated(Guid agentId, PageParameters pageParameters)
        {
            // Retrieve customers associated with the given AgentId
            var customers = _repository.GetAll()
                .Include(c => c.User)
                .Include(a => a.Address)
                    .ThenInclude(ad => ad.City)
                        .ThenInclude(city => city.State)
                .Include(c => c.Policies)
                .Where(c => c.AgentId == agentId)
                .ToList();

            // Check if any customers are found
            if (!customers.Any())
                throw new CustomerNotFoundException("No customers found for the given AgentId.");

            // Paginate and map the customers to CustomerResponseDto
            var pagedCustomers = PageList<CustomerResponseDto>.ToPagedList(
                _mapper.Map<List<CustomerResponseDto>>(customers),
                pageParameters.PageNumber,
                pageParameters.PageSize
            );
            Log.Information("Records Retrieved. Count: {count}", customers.Count);// Log the count of records retrieved


            return pagedCustomers;
        }

        public List<CustomerResponseDto> GetAll()
        {
            var customers = _repository.GetAll()
                .Include(c => c.User)
                .Include(a => a.Agent)
                .Include(a => a.Address)
                    .ThenInclude(ad => ad.City)
                        .ThenInclude(city => city.State)
                .Include(c => c.Policies)
                .ToList();

            if (!customers.Any())
                throw new CustomersDoesNotExistException("No customers exist.");
            Log.Information("Records Retrieved. Count: {count}", customers.Count);// Log the count of records retrieved

            return _mapper.Map<List<CustomerResponseDto>>(customers);
        }
        public PageList<CustomerResponseDto> GetAllPaginated(PageParameters pageParameters)
        {
            var customers = _repository.GetAll().ToList();
            var pagedCustomers = PageList<CustomerResponseDto>.ToPagedList(
                _mapper.Map<List<CustomerResponseDto>>(customers),
                pageParameters.PageNumber,
                pageParameters.PageSize
            );
            Log.Information("Records Retrieved. Count: {count}", customers.Count);// Log the count of records retrieved
            return pagedCustomers;
        }

        public bool Update(CustomerRequestDto customerRequestDto)
        {
            if (customerRequestDto.CustomerId == null)
                throw new CustomerNotFoundException("CustomerId is required for update");

            var existingCustomer = _repository.GetAll()
                .Include(c => c.User)
                .Include(a => a.Address)
                    .ThenInclude(ad => ad.City)
                        .ThenInclude(city => city.State)
                .FirstOrDefault(c => c.CustomerId == customerRequestDto.CustomerId.Value);

            if (existingCustomer == null)
                throw new CustomerNotFoundException("No such customer found.");

            // Update basic fields
            _mapper.Map(customerRequestDto, existingCustomer);

            // Update Address, City, and State
            if (existingCustomer.Address == null)
            {
                existingCustomer.Address = new Address
                {
                    HouseNo = customerRequestDto.HouseNo,
                    Apartment = customerRequestDto.Apartment,
                    Pincode = customerRequestDto.Pincode,
                    City = new City
                    {
                        CityName = customerRequestDto.City,
                        State = new State
                        {
                            StateName = customerRequestDto.State
                        }
                    }
                };
            }
            else
            {
                existingCustomer.Address.HouseNo = customerRequestDto.HouseNo;
                existingCustomer.Address.Apartment = customerRequestDto.Apartment;
                existingCustomer.Address.Pincode = customerRequestDto.Pincode;

                if (existingCustomer.Address.City == null)
                {
                    existingCustomer.Address.City = new City
                    {
                        CityName = customerRequestDto.City,
                        State = new State
                        {
                            StateName = customerRequestDto.State
                        }
                    };
                }
                else
                {
                    existingCustomer.Address.City.CityName = customerRequestDto.City;
                    if (existingCustomer.Address.City.State == null)
                    {
                        existingCustomer.Address.City.State = new State
                        {
                            StateName = customerRequestDto.State
                        };
                    }
                    else
                    {
                        existingCustomer.Address.City.State.StateName = customerRequestDto.State;
                    }
                }
            }

            // Hash the password if it's being updated
            if (!string.IsNullOrEmpty(customerRequestDto.Password))
            {
                existingCustomer.User.Password = BCrypt.Net.BCrypt.HashPassword(customerRequestDto.Password);
            }

            _repository.Update(existingCustomer);
            Log.Information("Record upadted", existingCustomer.CustomerId);

            return true;
        }

        public bool Delete(Guid id)
        {
            var customer = _repository.GetById(id);
            if (customer != null)
            {
                _repository.Delete(customer);
                Log.Information("Record upadted", id);// Log the count of records retrieved
                return true;
            }
            throw new CustomerNotFoundException("No such customer found to delete");
        }
        public bool UpdateUsername(CustomerRequestDto customerRequestDto)
        {
            if (customerRequestDto.CustomerId == null)
                throw new AgentNotFoundException("AgentId is required for updating username.");

            var existingAgent = _repository.GetAll()
                .Include(a => a.User)
                .FirstOrDefault(a => a.CustomerId == customerRequestDto.CustomerId.Value);

            if (existingAgent == null)
                throw new AgentNotFoundException("No such agent found.");

            if (string.IsNullOrWhiteSpace(customerRequestDto.UserName))
                throw new ArgumentException("Username cannot be null or empty.");

            // Check if the username already exists
            var allAgents = _repository.GetAll();
            if (allAgents.Any(a => a.User.UserName.ToLower() == customerRequestDto.UserName.ToLower() && a.CustomerId != customerRequestDto.CustomerId))
                throw new ArgumentException("Username already exists.");

            // Update only the username
            existingAgent.User.UserName = customerRequestDto.UserName;

            _repository.Update(existingAgent);
            return true;
        }
    }
}
