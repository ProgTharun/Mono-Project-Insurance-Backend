using AutoMapper;
using InsurancePolicy.DTOs;
using InsurancePolicy.Exceptions.AdminExceptions;
using InsurancePolicy.Exceptions.AgentExceptions;
using InsurancePolicy.Exceptions.EmployeeExceptions;
using InsurancePolicy.Helpers;
using InsurancePolicy.Models;
using InsurancePolicy.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Stripe;
using System.ComponentModel.DataAnnotations;

namespace InsurancePolicy.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepository<Employee> _repository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IMapper _mapper;

        public EmployeeService(IRepository<Employee> repository, IRepository<Role> roleRepository, IMapper mapper)
        {
            _repository = repository;
            _roleRepository = roleRepository;
            _mapper = mapper;
        }


        public Guid Add(EmployeeRequestDto employeeRequestDto)
        {
            var errors = new List<string>();

            // Ensure the "Employee" role exists
            var employeeRole = _roleRepository.GetAll().FirstOrDefault(r => r.Name == "Employee");
            if (employeeRole == null)
                errors.Add("Employee role not found.");

            var existingUserByUsername = _repository.GetAll().FirstOrDefault(u => u.User.UserName == employeeRequestDto.UserName);
            if (existingUserByUsername != null)
                errors.Add("Username already exists.");

            // Check if email already exists in the Customers table
            var existingUserByEmail = _repository.GetAll().FirstOrDefault(u => u.Email == employeeRequestDto.Email);
            if (existingUserByEmail != null)
                errors.Add("Email already exists.");

            var existingUserByPhone = _repository.GetAll().FirstOrDefault(u => u.Phone == employeeRequestDto.Phone);
            if (existingUserByPhone != null)
                errors.Add("Mobile number already exists.");

            // If there are any validation errors, throw a ValidationException
            if (errors.Any())
                throw new ValidationException(string.Join("; ", errors));

            // Hash the password
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(employeeRequestDto.Password);

            // Map EmployeeRequestDto to Employee model
            var employee = _mapper.Map<Employee>(employeeRequestDto);

            // Assign RoleId and hashed password to User
            employee.User.Password = hashedPassword;
            employee.User.RoleId = employeeRole.Id;

            _repository.Add(employee);
            Log.Information("New record added:" + employee.EmployeeId);
            return employee.EmployeeId;
        }

        public EmployeeResponseDto GetById(Guid id)
        {
            var employee = _repository.GetAll()
                .Include(e => e.User)
                .FirstOrDefault(e => e.EmployeeId == id);

            if (employee == null)
                throw new EmployeeNotFoundException("No such employee found.");

            // Log the UserName to ensure it is being fetched
            Console.WriteLine($"Fetched UserName: {employee.User?.UserName}");
            Log.Information("Record Retrieved. ", employee.EmployeeId);

            return _mapper.Map<EmployeeResponseDto>(employee);
        }


        public List<EmployeeResponseDto> GetAll()
        {
            var employees = _repository.GetAll()
                .Include(e => e.User)
                .ToList();

            if (!employees.Any())
                throw new EmployeesDoesNotExistException("No employees exist.");
            Log.Information("Records Retrieved. Count{count}", employees.Count);

            return _mapper.Map<List<EmployeeResponseDto>>(employees);
        }

        public bool Update(EmployeeRequestDto employeeRequestDto)
        {
            if (employeeRequestDto.EmployeeId == null)
                throw new EmployeeNotFoundException("EmployeeId is required for update");

            var existingEmployee = _repository.GetAll()
                .Include(e => e.User)
                .FirstOrDefault(e => e.EmployeeId == employeeRequestDto.EmployeeId.Value);

            if (existingEmployee == null)
                throw new EmployeeNotFoundException("No such employee found.");

            // Map updated values to the existing employee entity
            _mapper.Map(employeeRequestDto, existingEmployee);

            // Hash the password if it's being updated
            if (!string.IsNullOrEmpty(employeeRequestDto.Password))
            {
                existingEmployee.User.Password = BCrypt.Net.BCrypt.HashPassword(employeeRequestDto.Password);
            }

            _repository.Update(existingEmployee);
            Log.Information("Record upadted", existingEmployee.EmployeeId);// Log the count of records retrieved

            return true;
        }
        public void UpdateSalary(Guid id, double salary)
        {
            var employee = _repository.GetAll()
                .Include(e => e.User) // Include User navigation property
                .FirstOrDefault(e => e.EmployeeId == id);

            if (employee == null)
                throw new EmployeeNotFoundException("No such employee found.");
            employee.Salary = salary;
            Log.Information("Record upadted", employee.EmployeeId);// Log the count of records retrieved

            _repository.Update(employee);

        }
        public bool UpdateUsername(EmployeeRequestDto employeeRequestDto)
        {
            if (employeeRequestDto.EmployeeId == null)
                throw new AgentNotFoundException("AgentId is required for updating username.");

            var existingEmployee = _repository.GetAll()
                .Include(a => a.User)
                .FirstOrDefault(a => a.EmployeeId == employeeRequestDto.EmployeeId.Value);

            if (existingEmployee == null)
                throw new AgentNotFoundException("No such agent found.");

            if (string.IsNullOrWhiteSpace(employeeRequestDto.UserName))
                throw new ArgumentException("Username cannot be null or empty.");

            // Check if the username already exists
            var allEmployees = _repository.GetAll();
            if (allEmployees.Any(a => a.User.UserName.ToLower() == employeeRequestDto.UserName.ToLower() && a.EmployeeId != employeeRequestDto.EmployeeId))
                throw new ArgumentException("Username already exists.");

            // Update only the username
            existingEmployee.User.UserName = employeeRequestDto.UserName;

            _repository.Update(existingEmployee);
            return true;
        }
        public void Activate(Guid id)
        {
            var employee = _repository.GetById(id);
            if (employee == null)
                throw new EmployeeNotFoundException("No such employee found to activate");
            Log.Information("Record upadted", employee.EmployeeId);// Log the count of records retrieved

            _repository.Activate(employee);
        }
        public PageList<EmployeeResponseDto> GetAllPaginated(PageParameters pageParameters)
        {
            var employees = _repository.GetAll().ToList();
            var pagedEmployees = PageList<EmployeeResponseDto>.ToPagedList(
                _mapper.Map<List<EmployeeResponseDto>>(employees),
                pageParameters.PageNumber,
                pageParameters.PageSize
            );
            Log.Information("Records Retrieved. Count{count}", employees.Count);

            return pagedEmployees;
        }
        
        public bool Delete(Guid id)
        {
            var employee = _repository.GetById(id);
            if (employee != null)
            {
                _repository.Delete(employee);
                Log.Information("Record upadted", id);// Log the count of records retrieved

                return true;
            }
            throw new EmployeeNotFoundException("No such employee found to delete");
        }

        public int GetTotalCount()
        {
            return _repository.GetAll().Count();
        }
    }

}
