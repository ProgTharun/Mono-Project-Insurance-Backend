using InsurancePolicy.enums;
using InsurancePolicy.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class Nominee
{
    [Key]
    public Guid NomineeId { get; set; }

    [Required(ErrorMessage = "Nominee name is required")]
    [StringLength(100, ErrorMessage = "Nominee name cannot exceed 100 characters.")]
    public string NomineeName { get; set; }

    [Required(ErrorMessage = "Nominee relationship is required")]
    public NomineeRelationship Relationship { get; set; }

    [ForeignKey("PolicyAccount")]
    public Guid? PolicyNo { get; set; } // Made optional
    public Policy PolicyAccount { get; set; } // Navigation property
}
