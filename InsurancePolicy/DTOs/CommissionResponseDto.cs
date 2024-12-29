using InsurancePolicy.enums;

namespace InsurancePolicy.DTOs
{
    public class CommissionResponseDto
    {
        public Guid CommissionId { get; set; }
        public string CommissionType { get; set; } // Change from enum to string
        public DateTime IssueDate { get; set; }
        public double Amount { get; set; }
        public Guid AgentId { get; set; }
        public string AgentName { get; set; } // Optional for display
        public Guid? PolicyNo { get; set; } // Nullable for optional linkage
        public string PolicyName { get; set; } // Optional for display
    }
}
