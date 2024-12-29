namespace InsurancePolicy.DTOs
{
    public class TaxSettingsResponseDto
    {
        public Guid TaxId { get; set; }
        public double TaxPercentage { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
