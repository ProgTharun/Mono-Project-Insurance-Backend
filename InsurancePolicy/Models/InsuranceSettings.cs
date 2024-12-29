using System.ComponentModel.DataAnnotations;

namespace InsurancePolicy.Models
{
    public class InsuranceSettings
    {
        [Key]
        public Guid Id { get; set; }

        [Range(0, 30, ErrorMessage = "Claim deduction percentage must be between 0 and 30%.")]
        public double ClaimDeductionPercentage { get; set; }

        [Range(0, 50, ErrorMessage = "Penalty deduction percentage must be between 0 and 50%.")]
        public double PenaltyDeductionPercentage { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

    }
}
