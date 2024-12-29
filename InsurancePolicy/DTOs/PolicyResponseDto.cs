using InsurancePolicy.enums;

namespace InsurancePolicy.DTOs
{
    public class PolicyResponseDto
    {
        public Guid PolicyId { get; set; }
        public string InsuranceSchemeName { get; set; } // From Navigation Property
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; } // From Customer Navigation Property
        public DateTime IssueDate { get; set; }
        public DateTime MaturityDate { get; set; }
        public string PremiumType { get; set; }
        public double SumAssured { get; set; }
        public long PolicyTerm { get; set; }
        public double PremiumAmount { get; set; }
        public double? InstallmentAmount { get; set; }
        public double? TotalPaidAmount { get; set; }
        public double Tax {  get; set; }
        public string? RejectedReason { get; set; }

        public string PolicyStatus { get; set; } // Enum value as string
        public string AgentName { get; set; } // From Agent Navigation Property
        public List<NomineeResponseDto> Nominees { get; set; }
        public List<DocumentResponseDto> Documents { get; set; }
        public List<InstallmentResponseDto> Installments { get; set; }
        public List<PaymentResponseDto> Payments { get; set; }

    }

}
