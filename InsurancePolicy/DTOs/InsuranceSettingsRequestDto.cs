using System.ComponentModel.DataAnnotations;

namespace InsurancePolicy.DTOs
{
    public class InsuranceSettingsRequestDto
    {
        [Range(0, 100, ErrorMessage = "Claim deduction percentage must be between 0 and 100.")]
        public double ClaimDeductionPercentage { get; set; }

        [Range(0, 100, ErrorMessage = "Penalty deduction percentage must be between 0 and 100.")]
        public double PenaltyDeductionPercentage { get; set; }
    }
}
