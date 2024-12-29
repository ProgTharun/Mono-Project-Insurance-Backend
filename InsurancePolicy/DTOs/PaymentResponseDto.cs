using InsurancePolicy.enums;

namespace InsurancePolicy.DTOs
{
    public class PaymentResponseDto
    {
        public Guid PaymentId { get; set; }
        public PaymentType PaymentType { get; set; }
        public double AmountPaid { get; set; }
        public double Tax { get; set; }
        public double TotalPayment { get; set; }
        public DateTime PaymentDate { get; set; }
        public Guid PolicyId { get; set; }
        public string PolicyName { get; set; } // Scheme or policy name for reference
    }

}
