using System;

namespace InsurancePolicy.DTOs
{
    public class ClaimResponseDto
    {
        public Guid ClaimId { get; set; }
        public Guid PolicyId { get; set; }
        public string PolicyName { get; set; } // Optional: Name of the policy
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; } // Optional: Name of the customer
        public double ClaimAmount { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankIFSCCode { get; set; }
        public string Status { get; set; } // Enum as a string
        public DateTime ClaimDate { get; set; }
        public string? ClaimReason { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public DateTime? RejectionDate { get; set; }
    }
}
