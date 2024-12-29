using System.ComponentModel.DataAnnotations;

namespace InsurancePolicy.DTOs
{
    public class NomineeResponseDto
    {
        public Guid? NomineeId { get; set; } // Optional for Update

        [Required(ErrorMessage = "Nominee name is required.")]
        [StringLength(100, ErrorMessage = "Nominee name cannot exceed 100 characters.")]
        public string NomineeName { get; set; }

        [Required(ErrorMessage = "Relationship is required.")]
        public string Relationship { get; set; } // Enum value as string (e.g., "SPOUSE", "CHILD")

    }

}
