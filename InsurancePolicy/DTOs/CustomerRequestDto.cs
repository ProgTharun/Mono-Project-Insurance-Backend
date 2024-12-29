using System.ComponentModel.DataAnnotations;

namespace InsurancePolicy.DTOs
{
    public class CustomerRequestDto
    {
        public Guid? CustomerId { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(50, ErrorMessage = "First Name should not exceed 50 characters.")]
        public string CustomerFirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        [StringLength(50, ErrorMessage = "Last Name should not exceed 50 characters.")]
        public string CustomerLastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(100, ErrorMessage = "Email should not exceed 100 characters.")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format.")]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Phone number must start with 6-9 and contain exactly 10 digits.")]
        public string Phone { get; set; }

        // Address Details
        [Required(ErrorMessage = "House Number is required.")]
        [StringLength(50, ErrorMessage = "House Number should not exceed 50 characters.")]
        public string HouseNo { get; set; }

        [Required(ErrorMessage = "Apartment name is required.")]
        [StringLength(50, ErrorMessage = "Apartment name should not exceed 50 characters.")]
        public string Apartment { get; set; }

        [Required(ErrorMessage = "Pincode is required.")]
        [Range(100000, 999999, ErrorMessage = "Pincode must have exactly 6 digits.")]
        public int Pincode { get; set; }

        [Required(ErrorMessage = "City is required.")]
        [StringLength(50, ErrorMessage = "City name should not exceed 50 characters.")]
        public string City { get; set; }

        [Required(ErrorMessage = "State is required.")]
        [StringLength(50, ErrorMessage = "State name should not exceed 50 characters.")]
        public string State { get; set; }

        public bool Status { get; set; }

        public Guid? AgentId { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters long.")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 50 characters long.")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.")]
        public string Password { get; set; } = string.Empty;
        [Required(ErrorMessage = "Age is required.")]
        [Range(18, 100, ErrorMessage = "Age must be between 18 and 100.")]
        public DateTime DateOfBirth { get; set; }

    }
}
