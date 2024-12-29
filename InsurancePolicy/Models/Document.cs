using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using InsurancePolicy.enums;

namespace InsurancePolicy.Models
{
    public class Document
    {
        [Key]
        public string DocumentId { get; set; }
                
        [Required(ErrorMessage = "Document Name is required.")]
        [StringLength(250, ErrorMessage = "Document Name should not exceed 100 characters.")]
        public DocumentType DocumentName { get; set; }

        [Required(ErrorMessage = "Document Name is required.")]
        public string DocumentPath { get; set; }

        public Status Status { get; set; }

        [Required(ErrorMessage = "Customer is required.")]
        [ForeignKey("Customer")]
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }

        [ForeignKey("VerifiedBy")]
        public Guid? VerifiedById { get; set; }
        public Employee? VerifiedBy { get; set; }
        public string? RejectedReason { get; set; }

        public Guid? PolicyId { get; set; }
        public Policy? Policy { get; set; }
    }
}
