using System;

namespace InsurancePolicy.DTOs
{
    public class CustomerQueryRequestDto
    {
        public Guid? QueryId { get; set; } // Optional for update
        public string Title { get; set; }
        public string Message { get; set; }
        public Guid CustomerId { get; set; }
        public string Type { get; set; }
        public Guid? PolicyId { get; set; }
    }
}
