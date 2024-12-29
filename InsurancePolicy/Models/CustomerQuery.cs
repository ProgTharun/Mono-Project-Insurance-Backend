using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InsurancePolicy.Models
{
    public class CustomerQuery
    {
        [Key]
        public Guid QueryId { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(255, ErrorMessage = "Title should not exceed 255 characters.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Message is required.")]
        [MaxLength] // No specific length for TEXT column
        public string Message { get; set; }

        [MaxLength] // No specific length for TEXT column
        public string? Response { get; set; }
        public string QueryType { get; set; }

        public bool IsResolved { get; set; } = false;

        [ForeignKey("ResolvedBy")]
        public Guid? ResolvedByEmployeeId { get; set; } // Nullable for unresolved queries
        public Employee? ResolvedBy { get; set; }

        [ForeignKey("Customer")]
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }

        [ForeignKey("Policy")]
        public Guid? PolicyId { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now; 
        public DateTime? ResolvedAt { get; set; } 

    }
}
