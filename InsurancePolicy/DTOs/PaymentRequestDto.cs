using InsurancePolicy.enums;
using System.ComponentModel.DataAnnotations;

namespace InsurancePolicy.DTOs
{
    public class PaymentRequestDto
    {
        public Guid? PaymentId { get; set; }

        [Required(ErrorMessage = "Payment Type is required.")]
        public PaymentType PaymentType { get; set; }

        [Required(ErrorMessage = "Amount Paid is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount Paid must be greater than 0.")]
        public double AmountPaid { get; set; }

        [Required(ErrorMessage = "Tax is required.")]
        [Range(0.0, double.MaxValue, ErrorMessage = "Tax must be a positive number.")]
        public double Tax { get; set; }

        [Required(ErrorMessage = "Total Payment is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total Payment must be greater than 0.")]
        public double TotalPayment { get; set; }

        [Required(ErrorMessage = "Payment Date is required.")]
        public DateTime PaymentDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Policy ID is required.")]
        public Guid PolicyId { get; set; }
    }

}
