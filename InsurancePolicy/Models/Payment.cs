using InsurancePolicy.enums;
using InsurancePolicy.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class Payment
{
    [Key]
    public Guid PaymentId { get; set; }

    [Required(ErrorMessage = "Payment type is required.")]
    public PaymentType paymentType { get; set; }

    [Required(ErrorMessage = "Amount paid is required.")]
    [Range(0.0, double.MaxValue, ErrorMessage = "Amount paid must be a positive number.")]
    public double AmountPaid { get; set; }

    [Required(ErrorMessage = "Tax is required.")]
    [Range(0.0, 18.0, ErrorMessage = "GST percentage must be between 0 and 18%.")]
    public double Tax { get; set; }

    [Required(ErrorMessage = "Total payment is required.")]
    [Range(0.0, double.MaxValue, ErrorMessage = "Total payment must be a positive number.")]
    public double TotalPayment { get; set; }

    [Required(ErrorMessage = "Payment date is required.")]
    [DataType(DataType.DateTime, ErrorMessage = "Invalid payment date.")]
    public DateTime PaymentDate { get; set; } = DateTime.Now;

    [ForeignKey("Policy")]
    public Guid? PolicyId { get; set; } // Made optional
    public Policy Policy { get; set; } // Navigation property
}
