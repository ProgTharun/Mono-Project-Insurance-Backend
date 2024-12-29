using InsurancePolicy.enums;
using InsurancePolicy.Models;
using System.ComponentModel.DataAnnotations;

namespace InsurancePolicy.DTOs
{
    public class InsuranceSchemeRequestDto
    {
        public Guid? SchemeId { get; set; }

        [Required(ErrorMessage = "Scheme Name is required.")]
        [StringLength(100, ErrorMessage = "Scheme Name should not exceed 100 characters.")]
        public string SchemeName { get; set; }

        [Required(ErrorMessage = "Scheme image is required.")]
        public string SchemeImage { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Minimum amount is required.")]
        [Range(500, double.MaxValue, ErrorMessage = "Minimum amount must be greater than 500.")]
        public double MinAmount { get; set; }

        [Required(ErrorMessage = "Maximum amount is required.")]
        [Range(1000, double.MaxValue, ErrorMessage = "Maximum amount must be greater than 1000.")]
        public double MaxAmount { get; set; }

        [Required(ErrorMessage = "Minimum investment time is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Minimum investment time must be at least 1 month.")]
        public int MinInvestTime { get; set; }

        [Required(ErrorMessage = "Maximum investment time is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Maximum investment time must be at least 1 month.")]
        public int MaxInvestTime { get; set; }

        [Required(ErrorMessage = "Minimum age is required.")]
        [Range(18, 100, ErrorMessage = "Minimum age must be between 18 and 70.")]
        public int MinAge { get; set; }

        [Required(ErrorMessage = "Maximum age is required.")]
        [Range(18, 100, ErrorMessage = "Maximum age must be between 18 and 70.")]
        public int MaxAge { get; set; }

        [Range(0.0, 25.0, ErrorMessage = "Profit ratio must be between 0% and 25%.")]
        public double ProfitRatio { get; set; }

        [Required(ErrorMessage = "Registration commission ratio is required.")]
        [Range(0.0, 20.0, ErrorMessage = "Registration commission ratio must be between 0 and 20%.")]
        public double RegistrationCommRatio { get; set; }

        [Required(ErrorMessage = "Installment commission ratio is required.")]
        [Range(0.0, 7.5, ErrorMessage = "Installment commission ratio must be between 0 and 7.5%.")]
        public double InstallmentCommRatio { get; set; }

        public bool Status { get; set; } = true;

        public Guid PlanId { get; set; }

        [Required]
        public List<DocumentType> RequiredDocuments { get; set; } = new List<DocumentType>();

        [Range(0, 15, ErrorMessage = "Claim deduction percentage must be between 0 and 15%.")]
        public double ClaimDeductionPercentage { get; set; }

        [Range(0, 20, ErrorMessage = "Penalty deduction percentage must be between 0 and 20%.")]
        public double PenaltyDeductionPercentage { get; set; }

    }
}
