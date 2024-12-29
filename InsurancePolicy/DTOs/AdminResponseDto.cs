namespace InsurancePolicy.DTOs
{
    public class AdminResponseDto
    {
        public Guid AdminId { get; set; }
        public string AdminFirstName { get; set; }
        public string AdminLastName { get; set; }
        public string AdminEmail { get; set; }
        public string AdminPhone { get; set; }
        public bool Status { get; set; }
        public string UserName { get; set; }
    }
}
