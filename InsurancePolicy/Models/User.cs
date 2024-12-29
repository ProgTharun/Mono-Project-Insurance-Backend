using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsurancePolicy.Models
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters long.")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(350, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 350 characters long.")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, " +
            "one number, and one special character.")]
        public string Password { get; set; } = string.Empty;

        [ForeignKey("Role")]
        public Guid RoleId { get;set; }
        public Role? Role { get; set; }
        public Customer? Customer { get; set; }
        public Admin? Admin { get; set; }
        public Agent? Agent { get; set; }
        public Employee? Employee { get; set; }


    }
}
