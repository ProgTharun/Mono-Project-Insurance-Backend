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
    public class PolicyController : ControllerBase
    {
        private readonly IPolicyService _service;

        public PolicyController(IPolicyService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var policy = _service.GetById(id);
            return Ok(policy);
        }

        [HttpPost, Authorize(Roles = "Agent,Customer")]
        public IActionResult Add(PolicyRequestDto policy)
        {
            
            var newPolicyId = _service.Add(policy);
            return Ok(newPolicyId);
        }

        [HttpPut]
        public IActionResult Update(PolicyRequestDto policy)
        {
            
            _service.Update(policy);
            return Ok("Policy updated successfully.");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _service.Delete(id);
            return Ok(new { Message = "Deleted Successfully!" });
        }
        [HttpGet]
        public IActionResult GetAll([FromQuery] PageParameters pageParameters)
        {
            var policies = _service.GetAll(pageParameters);

            Response.Headers.Add("X-Total-Count", policies.TotalCount.ToString());
            Response.Headers.Add("X-Total-Pages", policies.TotalPages.ToString());
            Response.Headers.Add("X-Current-Page", policies.CurrentPage.ToString());
            Response.Headers.Add("X-Has-Next", policies.HasNext.ToString());
            Response.Headers.Add("X-Has-Previous", policies.HasPrevious.ToString());

            return Ok(policies);
        }

        [HttpGet("agent/{agentId}"), Authorize(Roles = "Agent")]
        public IActionResult GetPoliciesByAgentId(Guid agentId, [FromQuery] PageParameters pageParameters)
        {
            var policies = _service.GetPoliciesByAgentId(agentId, pageParameters);

            Response.Headers.Add("X-Total-Count", policies.TotalCount.ToString());
            Response.Headers.Add("X-Total-Pages", policies.TotalPages.ToString());
            Response.Headers.Add("X-Current-Page", policies.CurrentPage.ToString());
            Response.Headers.Add("X-Has-Next", policies.HasNext.ToString());
            Response.Headers.Add("X-Has-Previous", policies.HasPrevious.ToString());

            return Ok(policies);
        }

        [HttpGet("customerId/{customerId}")]
        public IActionResult GetPoliciesByCustomer(Guid customerId)
        {
            var policies = _service.GetPoliciesByCustomer(customerId);

            return Ok(policies);
        }
        [HttpGet("customer/{customerId}")]
        public IActionResult GetPoliciesByCustomerId(Guid customerId, [FromQuery] PageParameters pageParameters)
        {
            var policies = _service.GetPoliciesByCustomerId(customerId, pageParameters);

            Response.Headers.Add("X-Total-Count", policies.TotalCount.ToString());
            Response.Headers.Add("X-Total-Pages", policies.TotalPages.ToString());
            Response.Headers.Add("X-Current-Page", policies.CurrentPage.ToString());
            Response.Headers.Add("X-Has-Next", policies.HasNext.ToString());
            Response.Headers.Add("X-Has-Previous", policies.HasPrevious.ToString());

            return Ok(policies);
        }
        [HttpGet("customer/pending/{customerId}")]
        public IActionResult GetPoliciesByCustomerIdPending(Guid customerId, [FromQuery] PageParameters pageParameters)
        {
            var policies = _service.GetPoliciesByCustomerIdPending(customerId, pageParameters);

            Response.Headers.Add("X-Total-Count", policies.TotalCount.ToString());
            Response.Headers.Add("X-Total-Pages", policies.TotalPages.ToString());
            Response.Headers.Add("X-Current-Page", policies.CurrentPage.ToString());
            Response.Headers.Add("X-Has-Next", policies.HasNext.ToString());
            Response.Headers.Add("X-Has-Previous", policies.HasPrevious.ToString());

            return Ok(policies);
        }
        [HttpGet("customer/active/{customerId}")]
        public IActionResult GetPoliciesByCustomerIdActive(Guid customerId, [FromQuery] PageParameters pageParameters)
        {
            var policies = _service.GetPoliciesByCustomerIdActive(customerId, pageParameters);

            Response.Headers.Add("X-Total-Count", policies.TotalCount.ToString());
            Response.Headers.Add("X-Total-Pages", policies.TotalPages.ToString());
            Response.Headers.Add("X-Current-Page", policies.CurrentPage.ToString());
            Response.Headers.Add("X-Has-Next", policies.HasNext.ToString());
            Response.Headers.Add("X-Has-Previous", policies.HasPrevious.ToString());

            return Ok(policies);
        }
        [HttpGet("pending"), Authorize(Roles = "Employee")]
        public IActionResult GetPoliciesPending([FromQuery] PageParameters pageParameters)
        {
            var policies = _service.GetPoliciesPending(pageParameters);

            Response.Headers.Add("X-Total-Count", policies.TotalCount.ToString());
            Response.Headers.Add("X-Total-Pages", policies.TotalPages.ToString());
            Response.Headers.Add("X-Current-Page", policies.CurrentPage.ToString());
            Response.Headers.Add("X-Has-Next", policies.HasNext.ToString());
            Response.Headers.Add("X-Has-Previous", policies.HasPrevious.ToString());

            return Ok(policies);
        }




        [HttpGet("scheme/{schemeId}")]
        public IActionResult GetPoliciesBySchemeId(Guid schemeId, [FromQuery] PageParameters pageParameters)
        {
            var policies = _service.GetPoliciesBySchemeId(schemeId, pageParameters);

            Response.Headers.Add("X-Total-Count", policies.TotalCount.ToString());
            Response.Headers.Add("X-Total-Pages", policies.TotalPages.ToString());
            Response.Headers.Add("X-Current-Page", policies.CurrentPage.ToString());
            Response.Headers.Add("X-Has-Next", policies.HasNext.ToString());
            Response.Headers.Add("X-Has-Previous", policies.HasPrevious.ToString());

            return Ok(policies);
        }
        [HttpGet("count")]
        public IActionResult GetCount()
        {
            var count = _service.GetCount();
            return Ok(new { count });
        }
        [HttpGet("CustomerCount")]
        public IActionResult GetPoliciesCountByCustomerId(Guid customerId)
        {
            var count=_service.GetPoliciesCountByCustomerId(customerId);
            return Ok(new { count });
        }

        [HttpGet("plan/{planId}")]
        public IActionResult GetPoliciesByPlanId(Guid planId, [FromQuery] PageParameters pageParameters)
        {
            var policies = _service.GetPoliciesByPlanId(planId, pageParameters);

            Response.Headers.Add("X-Total-Count", policies.TotalCount.ToString());
            Response.Headers.Add("X-Total-Pages", policies.TotalPages.ToString());
            Response.Headers.Add("X-Current-Page", policies.CurrentPage.ToString());
            Response.Headers.Add("X-Has-Next", policies.HasNext.ToString());
            Response.Headers.Add("X-Has-Previous", policies.HasPrevious.ToString());

            return Ok(policies);
        }
        [HttpPut("{policyId}/approve"), Authorize(Roles = "Employee")]
        public IActionResult ApprovePolicy(Guid policyId)
        {
            _service.ApprovePolicy(policyId);
            return Ok(new { Message = "Policy approved successfully." });
        }

        [HttpPut("{policyId}/reject"), Authorize(Roles = "Employee")]
        public IActionResult RejectPolicy(Guid policyId)
        {
            _service.RejectPolicy(policyId);
            return Ok(new { Message = "Policy rejected successfully." });
        }

        [HttpGet("policyByAgent/{id}")]
        public IActionResult GetPolicies(Guid id)
        {
            return Ok(new { count = _service.TotalPolicies(id) });
        }

    }
}