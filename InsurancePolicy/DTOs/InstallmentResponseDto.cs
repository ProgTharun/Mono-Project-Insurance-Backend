using InsurancePolicy.enums;

namespace InsurancePolicy.DTOs
{
    public class InstallmentResponseDto
    {
        public Guid InstallmentId { get; set; }
        public Guid PolicyId { get; set; }
        public string PolicyName { get; set; } // Scheme name or associated policy name
        public DateTime DueDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public double AmountDue { get; set; }
        public double? AmountPaid { get; set; }
        public InstallmentStatus Status { get; set; }
        public string? PaymentReference { get; set; }
    }

}
