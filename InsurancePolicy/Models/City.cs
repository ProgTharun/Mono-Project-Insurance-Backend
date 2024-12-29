using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InsurancePolicy.Models
{
    public class City
    {
        [Key]
        public Guid CityId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "City name should not exceed 100 characters.")]
        public string CityName { get; set; }

        [Required]
        [ForeignKey("State")]
        public Guid StateId { get; set; }

        public State? State { get; set; }
    }
}
