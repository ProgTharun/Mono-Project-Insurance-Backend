namespace InsurancePolicy.DTOs
{
    public class InsuranceSettingsResponseDto
    {
        public Guid Id { get; set; }
        public double ClaimDeductionPercentage { get; set; }
        public double PenaltyDeductionPercentage { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
