using System.ComponentModel.DataAnnotations;

namespace InsurancePolicy.Models
{
    public class State
    {
        [Key]
        public Guid StateId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "State name should not exceed 100 characters.")]
        public string StateName { get; set; }

        public List<City>? Cities { get; set; }
    }
}
