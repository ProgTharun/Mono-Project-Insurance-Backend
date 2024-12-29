using InsurancePolicy.enums;

namespace InsurancePolicy.DTOs
{
    public class InsuranceSchemeResponseDto
    {
        public Guid SchemeId { get; set; }
        public string SchemeName { get; set; }
        public string SchemeImage { get; set; }
        public string Description { get; set; }
        public double MinAmount { get; set; }
        public double MaxAmount { get; set; }
        public int MinInvestTime { get; set; }
        public int MaxInvestTime { get; set; }
        public int MinAge { get; set; }
        public int MaxAge { get; set; }
        public double ProfitRatio { get; set; }
        public double RegistrationCommRatio { get; set; }
        public double InstallmentCommRatio { get; set; }
        public bool Status { get; set; }
        public string? PlanName { get; set; }
        public double PenaltyDeductionPercentage { get; set; }
        public double ClaimDeductionPercentage { get; set; }
        public int PoliciesCount { get; set; }
        public List<DocumentType>? RequiredDocuments { get; set; }
    }

}
