using InsurancePolicy.enums;
using System.ComponentModel.DataAnnotations;

namespace InsurancePolicy.DTOs
{
    public class NomineeRequestDto
    {
        public Guid? NomineeId { get; set; }

        [Required(ErrorMessage = "Nominee Name is required.")]
        [StringLength(100, ErrorMessage = "Nominee Name cannot exceed 100 characters.")]
        public string NomineeName { get; set; }

        [Required(ErrorMessage = "Relationship is required.")]
        public NomineeRelationship Relationship { get; set; }

        [Required(ErrorMessage = "Policy ID is required.")]
        public Guid PolicyId { get; set; }
    }

}
