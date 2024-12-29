using System;
using System.ComponentModel.DataAnnotations;
using InsurancePolicy.enums;

namespace InsurancePolicy.DTOs
{
    public class ClaimRequestDto
    {
        public Guid? ClaimId { get; set; } // Optional for creation

        [Required(ErrorMessage = "Policy ID is required.")]
        public Guid PolicyId { get; set; }

        [Required(ErrorMessage = "Customer ID is required.")]
        public Guid CustomerId { get; set; }

        [Required(ErrorMessage = "Claim amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Claim amount must be greater than 0.")]
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
        public DateTime ClaimDate { get; set; }
        public string? RejectionReason { get; set; }
        public string? ClaimReason { get; set; }
    }
}
