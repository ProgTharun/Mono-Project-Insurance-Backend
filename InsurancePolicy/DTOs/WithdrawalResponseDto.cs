using InsurancePolicy.enums;
using System;

namespace InsurancePolicy.DTOs
{
    public class WithdrawalRequestResponseDto
    {
        public Guid WithdrawalRequestId { get; set; }
        public Guid? AgentId { get; set; }
        public string AgentName { get; set; }
        public Guid? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public WithdrawalRequestType RequestType { get; set; }
        public double Amount { get; set; }
        public DateTime RequestDate { get; set; }
        public WithdrawalRequestStatus Status { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public string RejectedReason { get; set; }
        public int TotalCustomers { get; set; }
        public int TotalPolicies { get; set; }
        public double TotalCommission { get; set; }
    }
}
