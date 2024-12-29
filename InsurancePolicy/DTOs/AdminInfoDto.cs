using System.ComponentModel.DataAnnotations;

namespace InsurancePolicy.DTOs
{
    public class AdminInfoDto
    {
        [Key]
        public Guid AdminId { get; set; }

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
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Invalid phone number. " +
            "It must start with 6-9 and contain exactly 10 digits.")]
        public string AdminPhone { get; set; }

    }
}
