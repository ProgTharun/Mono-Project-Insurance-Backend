namespace InsurancePolicy.DTOs
{
    public class InsurancePlanResponseDto
    {
        public Guid PlanId { get; set; }
        public string PlanName { get; set; }
        public bool Status { get; set; }
        public int TotalSchemes { get; set; }
        public List<string>? SchemeNames { get; set; }
    }
}
