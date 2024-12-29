using System.ComponentModel.DataAnnotations;

namespace InsurancePolicy.DTOs
{
    public class AdminRequestDto
    {
        public Guid? AdminId { get; set; }

        [Required(ErrorMessage = "Admin First Name is required.")]
        [StringLength(50, ErrorMessage = "Admin First Name should not exceed 50 characters.")]
        public string AdminFirstName { get; set; }

        [Required(ErrorMessage = "Admin Last Name is required.")]
        [StringLength(50, ErrorMessage = "Admin Last Name should not exceed 50 characters.")]
        public string AdminLastName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [StringLength(100, ErrorMessage = "Email should not exceed 100 characters.")]
        public string AdminEmail { get; set; }

        [Required(ErrorMessage = "Admin Phone is required.")]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Invalid phone number. It must start with 6-9 and contain exactly 10 digits.")]
        public string AdminPhone { get; set; }

        public bool Status { get; set; }

        // User fields
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters long.")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 50 characters long.")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, " +
                           "one number, and one special character.")]
        public string Password { get; set; } = string.Empty;
    }
}
