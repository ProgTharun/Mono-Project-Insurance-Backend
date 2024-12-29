using System;

namespace InsurancePolicy.DTOs
{
    public class CustomerQueryResponseDto
    {
        public Guid QueryId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string? Response { get; set; }
        public bool IsResolved { get; set; }
        public string? ResolvedBy { get; set; } // Employee name who resolved the query
        public string? CustomerName { get; set; } // Customer name
        public DateTime CreatedAt { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public Guid? PolicyId { get; set; }
        public string QueryType { get; set; }
    }
}
