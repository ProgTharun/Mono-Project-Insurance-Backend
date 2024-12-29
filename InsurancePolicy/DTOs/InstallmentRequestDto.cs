using InsurancePolicy.enums;
using System.ComponentModel.DataAnnotations;

namespace InsurancePolicy.DTOs
{
    public class InstallmentRequestDto
    {
        public Guid? InstallmentId { get; set; }

        public Guid? PolicyId { get; set; }

        [Required(ErrorMessage = "Due Date is required.")]
        public DateTime DueDate { get; set; }

        public DateTime? PaymentDate { get; set; }

        [Required(ErrorMessage = "Amount Due is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount Due must be greater than 0.")]
        public double AmountDue { get; set; }

        public double? AmountPaid { get; set; }

        public InstallmentStatus Status { get; set; } = InstallmentStatus.PENDING;

        public string? PaymentReference { get; set; }
    }

}
