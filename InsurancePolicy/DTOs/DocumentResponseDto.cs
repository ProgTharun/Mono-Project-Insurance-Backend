using InsurancePolicy.enums;

namespace InsurancePolicy.DTOs
{
    public class DocumentResponseDto
    {
        public string DocumentId { get; set; }
        public string DocumentName { get; set; }
        public string DocumentPath { get; set; }
        public Status Status { get; set; }
        public string CustomerName { get; set; } // Derived from Customer
        public string VerifiedByName { get; set; } // Derived from Employee
    }
}
