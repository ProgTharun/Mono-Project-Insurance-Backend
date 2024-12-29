namespace InsurancePolicy.DTOs
{
    public class AgentResponseDto
    {
        public Guid AgentId { get; set; }
        public string AgentFirstName { get; set; }
        public string AgentLastName { get; set; }
        public string? Qualification { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public double CommissionEarned { get; set; }
        public bool Status { get; set; }
        public string UserName { get; set; }
        public int TotalCustomers { get; set; }
        public int TotalPolicies { get; set; } // New field for Total Policies


    }
}
