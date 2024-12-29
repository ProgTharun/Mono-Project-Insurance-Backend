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
    public class CommissionController : ControllerBase
    {
        private readonly ICommissionService _commissionService;

        public CommissionController(ICommissionService commissionService)
        {
            _commissionService = commissionService;
        }

        [HttpPost]
        public IActionResult AddCommission(CommissionRequestDto commissionDto)
        {
            var commissionId = _commissionService.AddCommission(commissionDto);
            return Ok(new { CommissionId = commissionId, Message = "Commission added successfully." });
        }

        [HttpGet("agent/{agentId}"), Authorize(Roles = "Agent")]
        public IActionResult GetCommissionsByAgentPaginated(Guid agentId, [FromQuery] PageParameters pageParameters)
        {
            var commissions = _commissionService.GetCommissionsByAgentPaginated(agentId, pageParameters);

            // Add pagination metadata to headers
            Response.Headers.Add("X-Total-Count", commissions.TotalCount.ToString());
            Response.Headers.Add("X-Page-Size", commissions.PageSize.ToString());
            Response.Headers.Add("X-Current-Page", commissions.CurrentPage.ToString());
            Response.Headers.Add("X-Total-Pages", commissions.TotalPages.ToString());
            Response.Headers.Add("X-Has-Next", commissions.HasNext.ToString());
            Response.Headers.Add("X-Has-Previous", commissions.HasPrevious.ToString());

            return Ok(commissions);
        }
        [HttpGet("count")]
        public IActionResult CountCommission() {
            var count = _commissionService.GetCount();
            return Ok(new { Count = count }); 
        } 


        [HttpGet("policy/{policyId}")]
        public IActionResult GetCommissionsByPolicy(Guid policyId)
        {
            var commissions = _commissionService.GetCommissionsByPolicy(policyId);
            return Ok(commissions);
        }

        [HttpGet]
        public IActionResult GetAllCommissionsPaginated([FromQuery] PageParameters pageParameters)
        {
            var commissions = _commissionService.GetAllCommissionsPaginated(pageParameters);

            // Add pagination metadata to headers
            Response.Headers.Add("X-Total-Count", commissions.TotalCount.ToString());
            Response.Headers.Add("X-Page-Size", commissions.PageSize.ToString());
            Response.Headers.Add("X-Current-Page", commissions.CurrentPage.ToString());
            Response.Headers.Add("X-Total-Pages", commissions.TotalPages.ToString());
            Response.Headers.Add("X-Has-Next", commissions.HasNext.ToString());
            Response.Headers.Add("X-Has-Previous", commissions.HasPrevious.ToString());

            return Ok(commissions);
        }

    }
}
