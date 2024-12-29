using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InsurancePolicy.Models
{
    public class Address
    {
        [Key]
        public Guid AddressId { get; set; } // Changed to Guid for consistency with IDs

        [Required(ErrorMessage = "House number is required.")]
        public string HouseNo { get; set; }

        [Required(ErrorMessage = "Apartment name is required.")]
        public string Apartment { get; set; }

        [Required(ErrorMessage = "Pincode is required.")]
        [Range(100000, 999999, ErrorMessage = "Pincode must have 6 digits.")]
        public int Pincode { get; set; }

        [ForeignKey("City")]
        public Guid CityId { get; set; } // Foreign key to City
        public City City { get; set; }
    }
}
