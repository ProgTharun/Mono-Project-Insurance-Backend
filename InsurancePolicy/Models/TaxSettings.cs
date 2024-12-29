using System.ComponentModel.DataAnnotations;

namespace InsurancePolicy.Models
{
    public class TaxSettings
    {
        [Key]
        public Guid TaxId { get; set; }

        [Required(ErrorMessage = "Tax Percentage is required.")]
        [Range(0.0, 18.0, ErrorMessage = "Tax percentage must be between 0 and 18%.")]
        public double TaxPercentage { get; set; }

        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
