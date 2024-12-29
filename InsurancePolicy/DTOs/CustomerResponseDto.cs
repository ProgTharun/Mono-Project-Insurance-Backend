public class CustomerResponseDto
{
    public Guid CustomerId { get; set; }
    public string CustomerFirstName { get; set; }
    public string CustomerLastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string? HouseNo { get; set; }
    public string? Apartment { get; set; }
    public int Pincode { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public bool Status { get; set; }
    public string UserName { get; set; }
    public string? AgentName { get; set; }
    public int TotalPolicies { get; set; }
    public DateTime DateOfBirth { get; set; }
}
