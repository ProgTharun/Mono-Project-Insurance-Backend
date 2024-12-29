using System.ComponentModel.DataAnnotations;

namespace InsurancePolicy.DTOs
{
    public class PasswordChangeDto
    {
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters long.")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(350, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 350 characters long.")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, " +
            "one number, and one special character.")]
        public string OldPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(350, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 350 characters long.")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, " +
            "one number, and one special character.")]
        public string NewPassword { get; set; } = string.Empty;
    }
}
