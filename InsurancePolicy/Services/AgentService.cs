using AutoMapper;
using InsurancePolicy.DTOs;
using InsurancePolicy.Exceptions.AgentExceptions;
using InsurancePolicy.Exceptions.PlanExceptions;
using InsurancePolicy.Helpers;
using InsurancePolicy.Models;
using InsurancePolicy.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.ComponentModel.DataAnnotations;

namespace InsurancePolicy.Services
{
    public class AgentService : IAgentService
    {
        private readonly IRepository<Agent> _repository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public AgentService(
            IRepository<Agent> repository,
            IRepository<Role> roleRepository,
            IMapper mapper)
        {
            _repository = repository;
            _roleRepository = roleRepository;
            _mapper = mapper;
        }
        public int TotalAgents()
        {
            var count = _repository.GetAll().Count();
            Log.Information("Records Retrieved. Count: {count}", count);// Log the count of records retrieved
            return count;
        }
        public int GetAgentCountByCustomerId(Guid customerId)
        {
            var count= _repository.GetAll().Where(a => a.Customers.Any(c => c.CustomerId == customerId)).Count();
            Log.Information("Records Retrieved. Count: {count}", count);// Log the count of records retrieved
            return count;

        }
        public PageList<AgentResponseDto> GetAllPaginated(PageParameters pageParameters)
        {
            var agents = _repository.GetAll().Include(c=>c.Customers).ToList();
            var paginatedAgents = PageList<AgentResponseDto>.ToPagedList(
                _mapper.Map<List<AgentResponseDto>>(agents),
                pageParameters.PageNumber,
                pageParameters.PageSize
            );
            Log.Information("Records Retrieved. Count: {count}", paginatedAgents.Count);// Log the count of records retrieved
            return paginatedAgents;
        }
        public Guid Add(AgentRequestDto agentRequestDto)
        {
            var errors = new List<string>();

            // Ensure the "Agent" role exists
            var agentRole = _roleRepository.GetAll().FirstOrDefault(r => r.Name == "Agent");
            if (agentRole == null)
                errors.Add("Agent role not found.");

            // Check if username already exists
            var existingUserByUsername = _repository.GetAll().FirstOrDefault(u => u.User.UserName == agentRequestDto.UserName);
            if (existingUserByUsername != null)
                errors.Add("Username already exists.");

            // Check if email already exists
            var existingUserByEmail = _repository.GetAll().FirstOrDefault(u => u.Email == agentRequestDto.Email);
            if (existingUserByEmail != null)
                errors.Add("Email already exists.");

            var existingUserByPhone = _repository.GetAll().FirstOrDefault(u => u.Phone == agentRequestDto.Phone);
            if (existingUserByPhone != null)
                errors.Add("Mobile number already exists.");

            // If there are any validation errors, throw a ValidationException
            if (errors.Any())
                throw new ValidationException(string.Join("; ", errors));

            // Hash the password
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(agentRequestDto.Password);

            // Map the AgentRequestDto to the Agent model
            var agent = _mapper.Map<Agent>(agentRequestDto);

            // Set the hashed password and assign the "Agent" role ID
            agent.User.Password = hashedPassword;
            agent.User.RoleId = agentRole.Id;

            // Add the agent to the repository
            _repository.Add(agent);
            Log.Information("New Record Added:",agent.AgentId);// Log the count of records retrieved

            return agent.AgentId;
        }

        public AgentResponseDto GetById(Guid id)
        {
            var agent = _repository.GetAll()
                .Include(a => a.User) // Include User navigation property
                .Include(a => a.Customers) // Include Customers navigation property
                .FirstOrDefault(a => a.AgentId == id);

            if (agent == null)
                throw new AgentNotFoundException("No such agent found");
            
            Log.Information("Record Retrieved :", agent.AgentId);// Log the count of records retrieved
            return _mapper.Map<AgentResponseDto>(agent);
        }

        public List<AgentResponseDto> GetAll()
        {
            var agents = _repository.GetAll()
                .Include(a => a.User) // Include User navigation property
                .Include(a => a.Customers).ThenInclude(customer => customer.Policies) // Include Customers navigation property
                .ToList();

            if (!agents.Any())
                throw new AgentsDoesNotExistException("No agents exist");
            
            Log.Information("Records Retrieved. Count: {count}", agents.Count);// Log the count of records retrieved
            return _mapper.Map<List<AgentResponseDto>>(agents);
        }

        public bool Update(AgentRequestDto agentRequestDto)
        {
            if (agentRequestDto.AgentId == null)
                throw new AgentNotFoundException("AgentId is required for update");

            var existingAgent = _repository.GetAll()
                .Include(a => a.User)
                .Include(a => a.Customers)
                .FirstOrDefault(a => a.AgentId == agentRequestDto.AgentId.Value);

            if (existingAgent == null)
                throw new AgentNotFoundException("No such agent found");

            // Map updated values to the existing agent entity
            _mapper.Map(agentRequestDto, existingAgent);

            // Hash the password if it's being updated
            if (!string.IsNullOrEmpty(agentRequestDto.Password))
            {
                existingAgent.User.Password = BCrypt.Net.BCrypt.HashPassword(agentRequestDto.Password);
            }

            // Preserve the existing RoleId
            var agentRole = _roleRepository.GetAll().FirstOrDefault(r => r.Name == "Agent");
            if (agentRole != null)
            {
                existingAgent.User.RoleId = agentRole.Id;
            }

            _repository.Update(existingAgent);
            Log.Information("Record Updated", existingAgent.AgentId);// Log the count of records retrieved

            return true;
        }
        public bool UpdateUsername(AgentRequestDto agentRequestDto)
        {
            if (agentRequestDto.AgentId == null)
                throw new AgentNotFoundException("AgentId is required for updating username.");

            var existingAgent = _repository.GetAll()
                .Include(a => a.User)
                .FirstOrDefault(a => a.AgentId == agentRequestDto.AgentId.Value);

            if (existingAgent == null)
                throw new AgentNotFoundException("No such agent found.");

            if (string.IsNullOrWhiteSpace(agentRequestDto.UserName))
                throw new ArgumentException("Username cannot be null or empty.");

            // Check if the username already exists
            var allAgents = _repository.GetAll();
            if (allAgents.Any(a => a.User.UserName.ToLower() == agentRequestDto.UserName.ToLower() && a.AgentId != agentRequestDto.AgentId))
                throw new ArgumentException("Username already exists.");

            // Update only the username
            existingAgent.User.UserName = agentRequestDto.UserName;

            _repository.Update(existingAgent);
            return true;
        }
        public void Activate(Guid id)
        {
            var agent = _repository.GetById(id);
            if (agent == null)
                throw new AgentNotFoundException("No such agent found to activate");

            _repository.Activate(agent);
            Log.Information("Record Updated:", agent.AgentId);// Log the count of records retrieved

        }


        public List<AgentResponseDto> GetAgentsByCustomerId(Guid customerId)
        {
            // Find agents associated with the given customerId
            var agents = _repository.GetAll()
                .Include(a => a.Customers) // Include Customers navigation property
                .Where(a => a.Customers.Any(c => c.CustomerId == customerId))
                .ToList();

            if (!agents.Any())
                throw new AgentNotFoundException("No agents found for the specified customer.");
            
            Log.Information("Records Retrieved. Count: {count}", agents.Count);// Log the count of records retrieved
            return _mapper.Map<List<AgentResponseDto>>(agents);
        }
        public PageList<AgentResponseDto> GetAgentsByCustomerId(Guid customerId, PageParameters pageParameters)
        {
            // Fetch agents associated with the given customerId
            var agents = _repository.GetAll()
                .Include(a => a.Customers) // Include Customers navigation property
                .Where(a => a.Customers.Any(c => c.CustomerId == customerId))
                .ToList();

            if (!agents.Any())
                throw new AgentNotFoundException("No agents found for the specified customer.");

            // Apply pagination
            var paginatedAgents = PageList<AgentResponseDto>.ToPagedList(
                _mapper.Map<List<AgentResponseDto>>(agents),
                pageParameters.PageNumber,
                pageParameters.PageSize
            );
            Log.Information("Records Retrieved: Count:{ount}", agents.Count);
            return paginatedAgents;
        }

        public bool Delete(Guid id)
        {
            var agent = _repository.GetById(id);
            if (agent != null)
            {
                _repository.Delete(agent);
                Log.Information("Record upadted",id);// Log the count of records retrieved

                return true;
            }
            throw new AgentNotFoundException("No such agent found to delete");

        }
        public int TotalCustomers(Guid agentId)
        {
            var count = _repository
         .GetAll()
         .Where(a => a.AgentId == agentId) // Filter for the specific agent
         .SelectMany(a => a.Customers) // Flatten the Customers collection
         .Count();
            Log.Information("Records Retrieved. Count: {count}", count);// Log the count of records retrieved
            return count;
        }

        
        public int TotalCommissions(Guid agentId)
        {
            var commissionCount = _repository
                .GetAll()
                .Where(a => a.AgentId == agentId) // Filter for the specific agent
                .SelectMany(a => a.Commissions) // Flatten the Commissions collection
                .Count(); // Count the total commissions
            Log.Information("Records Retrieved. Count: {count}", commissionCount);// Log the count of records retrieved
            return commissionCount;
        }
    }
}
