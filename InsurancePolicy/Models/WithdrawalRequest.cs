using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using InsurancePolicy.enums;

namespace InsurancePolicy.Models
{
    public class WithdrawalRequest
    {
        [Key]
        public Guid WithdrawalRequestId { get; set; } 

        [ForeignKey("Agent")]
        public Guid? AgentId { get; set; } 
        public Agent Agent { get; set; }

        [ForeignKey("Customer")]
        public Guid? CustomerId { get; set; } 
        public Customer Customer { get; set; }

        [Required(ErrorMessage = "Request Type is required.")]
        public WithdrawalRequestType RequestType { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
        public double Amount { get; set; }

        [Required(ErrorMessage = "Request Date is required.")]
        public DateTime RequestDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Status is required.")]
        public WithdrawalRequestStatus Status { get; set; }
        public string? RejectedReason { get; set; }
        public DateTime? ApprovedAt { get; set; }

        [NotMapped] // This field can be dynamically computed or populated
        public double TotalCommission { get; set; }

    }
}
