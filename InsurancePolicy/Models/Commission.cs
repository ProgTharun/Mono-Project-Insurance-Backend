using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using InsurancePolicy.enums;

namespace InsurancePolicy.Models
{
    public class Commission
    {
        [Key]
        public Guid CommissionId { get; set; }

        [Required(ErrorMessage = "Commission type is required.")]
        public CommissionType CommissionType { get; set; }

        [Required(ErrorMessage = "Issue date is required.")]
        public DateTime IssueDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
        public double Amount { get; set; }

        [Required(ErrorMessage = "Agent is required.")]
        [ForeignKey("Agent")]
        public Guid AgentId { get; set; }
        public Agent Agent { get; set; }

        [ForeignKey("PolicyAccount")]
        public Guid? PolicyNo { get; set; } // Nullable for optional linkage to a policy
        public Policy PolicyAccount { get; set; }

    }
}
