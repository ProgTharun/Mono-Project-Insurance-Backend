using InsurancePolicy.enums;
using System.ComponentModel.DataAnnotations;

namespace InsurancePolicy.DTOs
{
    public class PolicyRequestDto
    {
        public Guid? PolicyId { get; set; } // Optional for Update

        [Required(ErrorMessage = "Insurance Scheme ID is required.")]
        public Guid InsuranceSchemeId { get; set; }

        [Required(ErrorMessage = "Customer ID is required.")]
        public Guid CustomerId { get; set; }

        [Required(ErrorMessage = "Maturity Date is required.")]
        public DateTime MaturityDate { get; set; }

        [Required(ErrorMessage = "Premium Type is required.")]
        public string PremiumType { get; set; } // Enum value as string (e.g., "INSTALLMENT", "FULL")

        [Required(ErrorMessage = "Sum Assured is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Sum Assured must be greater than 0.")]
        public double SumAssured { get; set; }

        [Required(ErrorMessage = "Policy Term is required.")]
        public long PolicyTerm { get; set; } // In months

        [Required(ErrorMessage = "Premium Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Premium Amount must be greater than 0.")]
        public double PremiumAmount { get; set; }

        public double? InstallmentAmount { get; set; } // Auto-calculated for Installments
        public Guid? AgentId { get; set; } // Optional if policy is not linked to an agent

        public Guid? TaxId { get; set; } // Optional TaxSettings ID
        public string? RejectedReason { get; set; } = "-";
        //public Guid? InsuranceSettingId { get; set; } // Optional InsuranceSettings ID

        public List<NomineeRequestDto>? Nominees { get; set; } // List of nominees
        public List<string>? SelectedDocumentIds { get; set; }
    }
}
