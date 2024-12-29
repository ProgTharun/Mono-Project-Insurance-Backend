using InsurancePolicy.DTOs;
using InsurancePolicy.Helpers;
using InsurancePolicy.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace InsurancePolicy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IPolicyService _policyService;

        public PaymentController(IPaymentService paymentService,IPolicyService policyService)
        {
            _paymentService = paymentService;
            _policyService= policyService;
        }

        [HttpGet("count")]
        public IActionResult GetCount()
        {
            var count = _paymentService.GetCount();
            return Ok(new { count });
        }
        [HttpPost]
        public async Task<IActionResult> AddPaymentAsync(PaymentRequestDto paymentDto)
        {
            var paymentId = _paymentService.AddPayment(paymentDto);


            var subject = "Payment Successful - e-Insurance";
            var customerEmail = _policyService.GetEmail(paymentDto.PolicyId);
            var body = $@"
            <p>Dear Customer,</p>
            <p>We are pleased to inform you that your payment has been successfully processed.</p>
            <p><b>Payment Details:</b></p>
            <ul>
                <li>Policy ID:{paymentDto.PolicyId}</li>
                <li>Payment ID: {paymentId}</li>
                <li>Amount Paid: ₹{paymentDto.AmountPaid}</li>
                <li>Total Payment: ₹{paymentDto.TotalPayment}</li>
                <li>Payment Date: {paymentDto.PaymentDate.ToString("yyyy-MM-dd HH:mm:ss")}</li>
            </ul>
            <p>Thank you for choosing our service. If you have any questions, feel free to contact our support team.</p>
            <p>Best regards,<br/>e-Insurance Team</p>";

            // Send the email
            var emailService = new EmailService();
            await emailService.SendEmailAsync(customerEmail, subject, body);

            return Ok(new { PaymentId = paymentId, Message = "Payment added successfully." });
        }

        [HttpGet("policy/{policyId}")]
        public IActionResult GetPaymentsByPolicy(Guid policyId)
        {
            var payments = _paymentService.GetPaymentsByPolicy(policyId);
            return Ok(payments);
        }
        [HttpGet, Authorize(Roles = "Admin")]
        public IActionResult GetAllPayments([FromQuery] PageParameters pageParameters)
        {
            var payments = _paymentService.GetAllPaginated(pageParameters);

            // Add pagination metadata to headers
            Response.Headers.Add("X-Total-Count", payments.TotalCount.ToString());
            Response.Headers.Add("X-Page-Size", payments.PageSize.ToString());
            Response.Headers.Add("X-Current-Page", payments.CurrentPage.ToString());
            Response.Headers.Add("X-Total-Pages", payments.TotalPages.ToString());
            Response.Headers.Add("X-Has-Next", payments.HasNext.ToString());
            Response.Headers.Add("X-Has-Previous", payments.HasPrevious.ToString());

            // Return the paginated payments in the response body
            return Ok(payments);
        }

    }
}
