using InsurancePolicy.enums;

namespace InsurancePolicy.DTOs
{
    public class DocumentDto
    {
        public Guid DocumentId { get; set; }
        public DocumentType DocumentName { get; set; }
        public string DocumentPath { get; set; }
        public Status Status { get; set; }
        public Guid UploadedById { get; set; } // For Admin, Agent, Customer, Employee
        public string UploadedByType { get; set; } // "Admin", "Agent", "Customer", "Employee"
        public Guid? VerifiedById { get; set; }
        public string VerifiedByName { get; set; }
    }
}
