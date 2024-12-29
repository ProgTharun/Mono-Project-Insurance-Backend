using InsurancePolicy.enums;

namespace InsurancePolicy.DTOs
{
    public class DocumentRequestDto
    {
        public string DocumentId { get; set; }
        public string DocumentName { get; set; }
        public string DocumentPath { get; set; }
        public Guid CustomerId { get; set; }
    }
}
