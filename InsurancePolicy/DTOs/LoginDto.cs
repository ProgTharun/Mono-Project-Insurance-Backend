using System.ComponentModel.DataAnnotations;

namespace InsurancePolicy.DTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Please provide your username.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be 3 to 50 characters long.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 50 characters long.")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, " +
            "one number, and one special character.")]
        public string Password { get; set; } = string.Empty;

    }
}
