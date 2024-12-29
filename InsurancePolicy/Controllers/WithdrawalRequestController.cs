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
    public class WithdrawalRequestController : ControllerBase
    {
        private readonly IWithdrawalRequestService _service;

        public WithdrawalRequestController(IWithdrawalRequestService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult CreateRequest(WithdrawalRequestDto requestDto)
        {
            var requestId = _service.CreateRequest(requestDto);
            return Ok(new { WithdrawalRequestId = requestId });
        }

        [HttpPut("{requestId}/approve"), Authorize(Roles = "Admin")]
        public IActionResult ApproveRequest(Guid requestId)
        {
            _service.ApproveRequest(requestId);
            return Ok(new { Message="Request approved successfully." });
        }

        [HttpPut("{requestId}/reject"), Authorize(Roles = "Admin")]
        public IActionResult RejectRequest(Guid requestId)
        {
            _service.RejectRequest(requestId);
            return Ok(new { Message = "Request rejected successfully." });
        }

        [HttpGet("{requestId}")]
        public IActionResult GetRequestById(Guid requestId)
        {
            var request = _service.GetRequestById(requestId);
            return Ok(request);
        }
        [HttpGet("agent/{agentId}/total-commission"), Authorize(Roles = "Agent")]
        public IActionResult GetTotalCommission(Guid agentId)
        {
            var totalCommission = _service.GetTotalCommission(agentId);
            return Ok(new { AgentId = agentId, TotalCommission = totalCommission });
        }
        [HttpGet("agent/{agentId}"), Authorize(Roles = "Agent")]
        public IActionResult GetRequestsByAgentIdPaginated(Guid agentId, [FromQuery] PageParameters pageParameters)
        {
            var requests = _service.GetRequestsByAgentIdPaginated(agentId, pageParameters);

            // Add pagination metadata to headers
            Response.Headers.Add("X-Total-Count", requests.TotalCount.ToString());
            Response.Headers.Add("X-Page-Size", requests.PageSize.ToString());
            Response.Headers.Add("X-Current-Page", requests.CurrentPage.ToString());
            Response.Headers.Add("X-Total-Pages", requests.TotalPages.ToString());
            Response.Headers.Add("X-Has-Next", requests.HasNext.ToString());
            Response.Headers.Add("X-Has-Previous", requests.HasPrevious.ToString());

            return Ok(requests);
        }

        [HttpGet("count")]
        public IActionResult GetCount()
        {
            var count = _service.GetCount();
            return Ok(new { count });
        }


        [HttpGet]
        public IActionResult GetAllRequests()
        {
            var requests = _service.GetAllRequests();
            return Ok(requests);
        }
    }
}
