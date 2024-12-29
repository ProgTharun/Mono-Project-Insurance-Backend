using System.ComponentModel.DataAnnotations;

namespace InsurancePolicy.Models
{
    public class InsurancePlan
    {
        [Key]
        public Guid PlanId { get; set; }

        [Required(ErrorMessage = "Plan Name is required.")]
        [StringLength(100, ErrorMessage = "Plan Name should not exceed 100 characters.")]
        public string PlanName { get; set; }

        public bool Status { get; set; } 
        public List<InsuranceScheme>? Schemes { get; set; }
    }
}
