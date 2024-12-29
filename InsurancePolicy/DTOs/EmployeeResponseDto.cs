namespace InsurancePolicy.DTOs
{
    public class EmployeeResponseDto
    {
        public Guid EmployeeId { get; set; }
        public string EmployeeFirstName { get; set; }
        public string EmployeeLastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public double Salary { get; set; }
        public bool Status { get; set; }
        public string UserName { get; set; }
    }
}
