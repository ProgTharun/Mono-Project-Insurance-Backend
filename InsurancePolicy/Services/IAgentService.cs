using InsurancePolicy.DTOs;
using InsurancePolicy.Helpers;
using InsurancePolicy.Models;

namespace InsurancePolicy.Services
{
    public interface IAgentService
    {
        public List<AgentResponseDto> GetAll();
        public AgentResponseDto GetById(Guid id);
        public Guid Add(AgentRequestDto agent);
        public bool Update(AgentRequestDto agent);
        public bool Delete(Guid id);
        public void Activate(Guid agentId);
        public int TotalAgents();
        public bool UpdateUsername(AgentRequestDto agentRequestDto);
        List<AgentResponseDto> GetAgentsByCustomerId(Guid customerId); // New method
        PageList<AgentResponseDto> GetAllPaginated(PageParameters pageParameters);
        public PageList<AgentResponseDto> GetAgentsByCustomerId(Guid customerId, PageParameters pageParameters);
        public int GetAgentCountByCustomerId(Guid customerId);
        public int TotalCustomers(Guid agentId);
        public int TotalCommissions(Guid agentId);
    }
}
