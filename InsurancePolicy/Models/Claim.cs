using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using InsurancePolicy.enums;

namespace InsurancePolicy.Models
{
    public class Claim
    {
        [Key]
        public Guid ClaimId { get; set; } 

        [Required(ErrorMessage = "PolicyId is required.")]
        [ForeignKey("Policy")]
        public Guid PolicyId { get; set; } 

        [Required(ErrorMessage = "CustomerId is required.")]
        [ForeignKey("Customer")]
        public Guid CustomerId { get; set; } 

        [Required(ErrorMessage = "Claim amount is required.")]
        [Range(100, double.MaxValue, ErrorMessage = "Claim amount must be a greater or equal to 100.")]
        public double ClaimAmount { get; set; } 

        [Required(ErrorMessage = "Bank account number is required.")]
        [StringLength(20, ErrorMessage = "Bank account number cannot exceed 20 characters.")]
        public string BankAccountNumber { get; set; } 

        [Required(ErrorMessage = "Bank IFSC code is required.")]
        [StringLength(15, ErrorMessage = "Bank IFSC code cannot exceed 15 characters.")]
        public string BankIFSCCode { get; set; } 

        [Required(ErrorMessage = "Claim status is required.")]
        public Status Status { get; set; } 

        [Required(ErrorMessage = "Claim date is required.")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid claim date.")]
        public DateTime ClaimDate { get; set; }
        public string? ClaimReason { get; set; }

        public string? RejectionReason { get; set; }
        public DateTime ApprovalDate { get; set; }
        public DateTime RejectionDate { get; set; }
        public Customer? Customer { get; set; }
        public Policy? Policy { get; set; }

    }
}
