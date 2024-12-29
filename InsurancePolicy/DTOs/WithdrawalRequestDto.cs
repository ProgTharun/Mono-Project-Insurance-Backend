using System;
using System.ComponentModel.DataAnnotations;
using InsurancePolicy.enums;

namespace InsurancePolicy.DTOs
{
    public class WithdrawalRequestDto
    {
        public Guid? WithdrawalRequestId { get; set; }

        [Required(ErrorMessage = "Agent ID is required.")]
        public Guid? AgentId { get; set; }

        public Guid? CustomerId { get; set; }

        [Required(ErrorMessage = "Request Type is required.")]
        public WithdrawalRequestType RequestType { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
        public double Amount { get; set; }

        public DateTime RequestDate { get; set; } = DateTime.Now;

        public WithdrawalRequestStatus Status { get; set; }

        public DateTime? ApprovedAt { get; set; }
        public string? RejectedReason { get; set; } // Nullable type


        public double TotalCommission { get; set; }
    }
}
