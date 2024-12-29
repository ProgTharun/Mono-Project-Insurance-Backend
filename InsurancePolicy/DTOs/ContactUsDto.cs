using System.ComponentModel.DataAnnotations;

public class ContactUsDto
{
    [Required]
    public string Name { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; }

    [Required]
    [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Phone number must start with 6-9 and contain exactly 10 digits.")]
    public string Phone { get; set; }
    public string Message { get; set; }

}