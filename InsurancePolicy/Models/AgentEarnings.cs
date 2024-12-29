using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InsurancePolicy.Models
{
    public class AgentEarnings
    {
        [Key]
        public Guid Id { get; set; } // Changed to Guid for consistency

        [Required]
        [ForeignKey("Agent")]
        public Guid AgentId { get; set; } // Foreign key for Agent
        public Agent Agent { get; set; } // Navigation property for Agent

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
        public double Amount { get; set; } // Amount earned by the agent

        [Required]
        public DateTime WithdrawalDate { get; set; } // Date of withdrawal

    }
}
