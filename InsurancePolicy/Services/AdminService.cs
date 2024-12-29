using AutoMapper;
using InsurancePolicy.DTOs;
using InsurancePolicy.Exceptions.AdminExceptions;
using InsurancePolicy.Exceptions.AgentExceptions;
using InsurancePolicy.Models;
using InsurancePolicy.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;

namespace InsurancePolicy.Services
{
    public class AdminService : IAdminService
    {
        private readonly IRepository<Admin> _repository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IMapper _mapper;

        public AdminService(
            IRepository<Admin> repository,
            IRepository<Role> roleRepository,
            IMapper mapper)
        {
            _repository = repository;
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        public Guid Add(AdminRequestDto adminRequestDto)
        {
            // Ensure the "Admin" role exists
            var adminRole = _roleRepository.GetAll().FirstOrDefault(r => r.Name == "Admin");
            if (adminRole == null)
                throw new Exception("Admin role not found."); // Handle this as a custom exception in production

            // Hash the password
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(adminRequestDto.Password);

            // Map the AdminRequestDto to the Admin model
            var admin = _mapper.Map<Admin>(adminRequestDto);

            // Set the hashed password and assign the "Admin" role ID
            admin.User.Password = hashedPassword;
            admin.User.RoleId = adminRole.Id; // Assign the RoleId fetched from the database

            // Add the admin to the repository
            _repository.Add(admin);

            Log.Information("New record added:" + admin.AdminId);
            // Return the Admin ID
            return admin.AdminId;
        }

        public AdminResponseDto GetById(Guid id)
        {
            var admin = _repository.GetAll().Include(a => a.User)
           .FirstOrDefault(a => a.AdminId == id);
            if (admin == null)
                throw new AdminNotFoundException("No such admin found");
            
            Log.Information("Record retrieved:" + admin.AdminId);
            return _mapper.Map<AdminResponseDto>(admin);
        }
        public AdminResponseDto GetByName(string name)
        {

            // Fetch the admin including the associated user
            var admin = _repository.GetAll()
                .Include(a => a.User)
                .Where(a => a.User.UserName == name).FirstOrDefault();

            if (admin == null)
                throw new AdminsDoesNotExistException($"Admin with username '{admin}' does not exist.");

            // Map the admin entity to AdminResponseDto
            Log.Information("Record retrived:" + admin.AdminId);
            return _mapper.Map<AdminResponseDto>(admin);
        }
        public List<AdminResponseDto> GetAll()
        {
            var admins = _repository.GetAll().Include(a => a.User).ToList();
            if (!admins.Any())
                throw new AdminsDoesNotExistException("No admins exist");
            // Log the count of records retrieved
            Log.Information("Admin records retrieved successfully. Count: {Count}", admins.Count);

            // Log IDs in debug mode for traceability
            //Log.Debug("Retrieved Admin IDs: {AdminIds}", admins.Select(a => a.AdminId));
            return _mapper.Map<List<AdminResponseDto>>(admins);
        }
        public bool UpdateUsername(AdminRequestDto adminRequestDto)
        {
            if (adminRequestDto.AdminId == null)
                throw new AgentNotFoundException("AgentId is required for updating username.");

            var existingAdmin = _repository.GetAll()
                .Include(a => a.User)
                .FirstOrDefault(a => a.AdminId == adminRequestDto.AdminId.Value);

            if (existingAdmin == null)
                throw new AgentNotFoundException("No such agent found.");

            if (string.IsNullOrWhiteSpace(adminRequestDto.UserName))
                throw new ArgumentException("Username cannot be null or empty.");

            // Check if the username already exists
            var allAgents = _repository.GetAll();
            if (allAgents.Any(a => a.User.UserName.ToLower() == adminRequestDto.UserName.ToLower() && a.AdminId != adminRequestDto.AdminId))
                throw new ArgumentException("Username already exists.");

            // Update only the username
            existingAdmin.User.UserName = adminRequestDto.UserName;

            _repository.Update(existingAdmin);
            return true;
        }
        public bool Update(AdminRequestDto adminRequestDto)
        {
            if (adminRequestDto.AdminId == null)
                throw new AdminNotFoundException("AdminId is required for update");

            var existingAdmin = _repository.GetAll()
                .Include(a => a.User)
                .FirstOrDefault(a => a.AdminId == adminRequestDto.AdminId.Value);

            if (existingAdmin == null)
                throw new AdminNotFoundException("No such admin found");

            // Map updated values to the existing admin entity
            _mapper.Map(adminRequestDto, existingAdmin);

            // Fetch the Admin role
            var adminRole = _roleRepository.GetAll().FirstOrDefault(r => r.Name == "Admin");
            if (adminRole == null)
                throw new Exception("Admin role not found.");

            // Update the User entity
            if (!string.IsNullOrEmpty(adminRequestDto.Password))
            {
                existingAdmin.User.Password = BCrypt.Net.BCrypt.HashPassword(adminRequestDto.Password);
            }
            existingAdmin.User.RoleId = adminRole.Id;            
            _repository.Update(existingAdmin);
            Log.Information("Record Updated:" + existingAdmin.AdminId);
            return true;
        }
    }
}
