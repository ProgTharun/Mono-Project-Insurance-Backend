using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InsurancePolicy.Models
{
    public class Agent
    {
        [Key]
        public Guid AgentId { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(50, ErrorMessage = "First Name cannot exceed 50 characters.")]
        public string AgentFirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        [StringLength(50, ErrorMessage = "Last Name cannot exceed 50 characters.")]
        public string AgentLastName { get; set; }

        [StringLength(100, ErrorMessage = "Qualification cannot exceed 100 characters.")]
        public string? Qualification { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Phone number must start with 6-9 and contain exactly 10 digits.")]
        public string Phone { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Commission Earned must be greater than 0.")]
        public double CommissionEarned { get; set; }

        public bool Status { get; set; }=true;

        [ForeignKey("User")]
        public Guid? UserId { get; set; }
        public User? User { get; set; }

        [ForeignKey("Address")]
        public Guid? AddressId { get; set; } // Foreign key for Address
        public Address? Address { get; set; } // Navigation property for Address

        [ForeignKey("Claim")]
        public List<Claim>? Claims { get; set; } // Navigation property for Claims

        public List<Commission>? Commissions { get; set; } // Navigation property for Commissions

        public List<Customer>? Customers { get; set; } // Navigation property for Customers


    }
}
