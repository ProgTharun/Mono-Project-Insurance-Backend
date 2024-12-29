using InsurancePolicy.enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsurancePolicy.Models
{
    public class Policy
    {
        [Key]
        public Guid PolicyId { get; set; }

        [ForeignKey("InsuranceScheme")]
        public Guid InsuranceSchemeId { get; set; }
        public InsuranceScheme InsuranceScheme { get; set; }

        [ForeignKey("Customer")]
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }

        [Required]
        public DateTime IssueDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Maturity Date is required.")]
        public DateTime MaturityDate { get; set; }

        [Required(ErrorMessage = "Premium Type is required.")]
        public PremiumType PremiumType { get; set; }

        [Required(ErrorMessage = "Sum Assured is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Sum Assured must be greater than 0.")]
        public double SumAssured { get; set; }

        [Required(ErrorMessage = "Policy Term is required.")]
        public long PolicyTerm { get; set; }

        [Required(ErrorMessage = "Premium Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Premium Amount must be greater than 0.")]
        public double PremiumAmount { get; set; }

        public double? InstallmentAmount { get; set; }
        public double? TotalPaidAmount { get; set; }

        public PolicyStatus Status { get; set; } = PolicyStatus.PENDING;

        [ForeignKey("Agent")]
        public Guid? AgentId { get; set; }
        public Agent Agent { get; set; }

        public List<Installment> Installments { get; set; }
        public List<Nominee> Nominees { get; set; }
        public List<Payment> Payments { get; set; }

        public List<Document> Documents { get; set; }

        [ForeignKey("TaxSetting")]
        public Guid? TaxId { get; set; }
        public TaxSettings TaxSettings { get; set; }

        public DateTime? CancellationDate { get; set; }

        //[ForeignKey("InsuranceSetting")]
        //public Guid? InsuranceSettingId { get; set; }
        //public InsuranceSettings InsuranceSettings { get; set; }
        public Claim Claim { get; set; }

    }
}
