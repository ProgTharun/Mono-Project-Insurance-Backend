using InsurancePolicy.DTOs;
using InsurancePolicy.Helpers;
using InsurancePolicy.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace InsurancePolicy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentController : ControllerBase
    {
        private readonly IAgentService _service;
        private readonly EmailService _emailService;

        public AgentController(IAgentService service, EmailService emailService)
        {
            _service = service;
            _emailService = emailService;
        }
        [HttpPut("activate"), Authorize(Roles ="Admin,Employee")]
        public IActionResult Activate(Guid id)
        {
            _service.Activate(id);
            return Ok(id);
        }
        [HttpGet("total")]
        public IActionResult TotalAgents()
        {
            var count = _service.TotalAgents();
            return Ok(new { Count = count, message = "total agents" });
        }
        [HttpGet("Agents/count")]
        public IActionResult GetAgentCountByCustomerId(Guid customerId)
        {
            var count = _service.GetAgentCountByCustomerId(customerId);
            return Ok(new { count });
        }
        [HttpPost("ContactUs")]
        public async Task<IActionResult> ContactUsAsync(ContactUsDto contactUsDTO)
        {
            var subject = "Thank You for Contacting E-Insurance";
            var body = $@"
                <p>Dear {contactUsDTO.Name},</p>
                <p>Thank you for reaching out to <b>E-Insurance</b>. We have received your message and will get back to you as soon as possible.</p>
                <p>Here are the details you provided:</p>
                <ul>
                <li><b>Name:</b> {contactUsDTO.Name}</li>
                <li><b>Email:</b> {contactUsDTO.Email}</li>
                <li><b>Phone:</b> {contactUsDTO.Phone}</li>
                <li><b>Message:</b> {contactUsDTO.Message}</li>
                </ul>
                <p>We take customer satisfaction seriously, and our dedicated team is here to assist you. One of our agents will contact you within 24-48 hours.</p>
                <p>If you have any urgent concerns, feel free to reach out to our customer support team dheerajkumar14.a@gmail.com or call us at <b>7013410921</b>.</p>
                <p>We appreciate your interest in E-Insurance and look forward to serving you.</p>
                <p>Best regards,<br/>E-Insurance Team</p>
                <hr/>
                <p><i>This is an automated message. Please do not reply to this email.</i></p>";

            await _emailService.SendEmailAsync(contactUsDTO.Email, subject, body);

            return Ok(new { message = "Email sent successfully." });
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] PageParameters pageParameters)
        {
            
            var agents = _service.GetAllPaginated(pageParameters);

            // Add pagination metadata to headers
            Response.Headers.Add("X-Total-Count", agents.TotalCount.ToString());
            Response.Headers.Add("X-Page-Size", agents.PageSize.ToString());
            Response.Headers.Add("X-Current-Page", agents.CurrentPage.ToString());
            Response.Headers.Add("X-Total-Pages", agents.TotalPages.ToString());
            Response.Headers.Add("X-Has-Next", agents.HasNext.ToString());
            Response.Headers.Add("X-Has-Previous", agents.HasPrevious.ToString());

            // Return the customers in the response body
            return Ok(agents);
        }
        [HttpGet("customer/{customerId}"), Authorize(Roles = "Customer")]
        public IActionResult GetAgentsByCustomerId(Guid customerId, [FromQuery] PageParameters pageParameters)
        {
            
            var agents = _service.GetAgentsByCustomerId(customerId, pageParameters);

            // Add pagination metadata to headers
            Response.Headers.Add("X-Total-Count", agents.TotalCount.ToString());
            Response.Headers.Add("X-Page-Size", agents.PageSize.ToString());
            Response.Headers.Add("X-Current-Page", agents.CurrentPage.ToString());
            Response.Headers.Add("X-Total-Pages", agents.TotalPages.ToString());
            Response.Headers.Add("X-Has-Next", agents.HasNext.ToString());
            Response.Headers.Add("X-Has-Previous", agents.HasPrevious.ToString());

            // Return paginated agents
            return Ok(agents);
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var agent = _service.GetById(id);
            return Ok(agent);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(AgentRequestDto agentRequestDto)
        {
            var newAgentId = _service.Add(agentRequestDto);
            var subject = "Account Created - e-Insurance";
            var body = $@"
            <p>Dear {agentRequestDto.AgentFirstName},</p>
            <p>Your account has been created successfully.</p>
            <p>The below are your Credentials generated by company. Use this to Login into our website.</p>
            <p>Your current Username is: <b>{agentRequestDto.UserName}</b></p>
            <p>Your current Password is: <b>Agent@123</b></p>
            <p>If you wish to change your password, please change it after login,in the Profile Section.</p>
            <p>Looking forward to working with you. :) </p>
            <p>Best regards,<br/>E-Insurance Team</p> ";

            // Send the email
            var emailService = new EmailService();
            await emailService.SendEmailAsync(agentRequestDto.Email, subject, body);
            return Ok(new { AgentId = newAgentId, Message = "Agent added successfully" });
        }

        [HttpPut]
        public IActionResult Modify(AgentRequestDto agentRequestDto)
        {
            _service.Update(agentRequestDto);
            return Ok(new { Message = "Agent updated successfully" });
        }
        [HttpDelete("{id}"), Authorize(Roles = "Admin,Employee")]
        public IActionResult Delete(Guid id)
        {
            
            _service.Delete(id);
            return Ok(new { Message="Deleted Successfully!" });
        }
        [HttpGet("customerByAgent/{id}")]
        public IActionResult GetCustomers(Guid id)
        {
            return Ok(new { count = _service.TotalCustomers(id) });
        }


        [HttpGet("commissionByAgent/{id}")]
        public IActionResult GetCommissions(Guid id)
        {
            return Ok(new { count = _service.TotalCommissions(id) });
        }
        [HttpPut("Profile")]
        public IActionResult UpdateUsername(AgentRequestDto agentRequestDto)
        {
            // Call the Update method in the service with the full DTO
            _service.UpdateUsername(agentRequestDto);

            return Ok(new { Message = "Username updated successfully." });
        }
    }
}
