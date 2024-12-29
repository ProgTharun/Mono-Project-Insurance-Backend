using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using InsurancePolicy.enums;

namespace InsurancePolicy.Models
{
    public class Installment
    {
        [Key]
        public Guid InstallmentId { get; set; }

        [ForeignKey("Policy")]
        public Guid? PolicyId { get; set; } // Made optional
        public Policy InsurancePolicy { get; set; } // Navigation property

        [Required]
        public DateTime DueDate { get; set; }

        public DateTime? PaymentDate { get; set; }

        [Required]
        public double AmountDue { get; set; }

        public double? AmountPaid { get; set; }

        public InstallmentStatus Status { get; set; } = InstallmentStatus.PENDING;

        public string? PaymentReference { get; set; }
    }

}
