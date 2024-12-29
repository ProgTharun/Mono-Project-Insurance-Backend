using System.ComponentModel.DataAnnotations;

namespace InsurancePolicy.Models
{
    public class Role
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "The name is required.")]
        [StringLength(50, ErrorMessage = "The RoleName should be less than or equal to 50 characters.")]
        public string Name { get; set; }

        public List<User>? Users { get; set; }
    }
}
