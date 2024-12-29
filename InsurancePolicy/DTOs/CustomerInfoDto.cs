using System.ComponentModel.DataAnnotations;

namespace InsurancePolicy.DTOs
{
    public class CustomerInfoDto
    {
        public Guid CustomerId { get; set; }

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

        [StringLength(100, ErrorMessage = "Address should not exceed 100 characters.")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "State is required.")]
        public string? State { get; set; }

        [StringLength(50, ErrorMessage = "City should not exceed 50 characters.")]
        public string? City { get; set; }

        [StringLength(100, ErrorMessage = "Nominee name should not exceed 100 characters.")]
        public string Nominee { get; set; }

        [StringLength(50, ErrorMessage = "Nominee relation should not exceed 50 characters.")]
        public string NomineeRelation { get; set; }
        public bool Status { get; set; }
    }
}
