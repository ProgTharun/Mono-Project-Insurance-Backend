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
    public class CustomerQueryController : ControllerBase
    {
        private readonly ICustomerQueryService _service;

        public CustomerQueryController(ICustomerQueryService service)
        {
            _service = service;
        }

        [HttpGet("count")]
        public IActionResult GetCount()
        {
            var count=_service.GetQueryCount();
            return Ok(new { count });
        }

        [HttpPost, Authorize(Roles = "Customer")]
        public IActionResult CreateQuery(CustomerQueryRequestDto queryDto)
        {
            var queryId = _service.AddQuery(queryDto);
            return Ok(new { QueryId = queryId });
        }
        [HttpGet("customercount")]
        public IActionResult GetComplaintCountByCustomerId(Guid customerId)
        {
            var count = _service.GetComplaintCountByCustomerId(customerId);
            return Ok(new { count });
        }

        [HttpPut]
        public IActionResult UpdateQuery(CustomerQueryRequestDto queryDto)
        {
            
            _service.UpdateQuery(queryDto);
            return Ok(new { Message = "Query updated successfully." });
        }

        [HttpPut("{queryId}/resolve"), Authorize(Roles = "Employee")]
        public IActionResult ResolveQuery(Guid queryId, [FromQuery] string response, [FromQuery] Guid employeeId)
        {
            _service.ResolveQuery(queryId, response, employeeId);
            return Ok(new { Message = "Query resolved successfully." });
        }

        [HttpGet("{queryId}")]
        public IActionResult GetQueryById(Guid queryId)
        {
            var query = _service.GetQueryById(queryId);
            return Ok(query);
        }

        [HttpGet("customer/{customerId}"), Authorize(Roles = "Customer")]
        public IActionResult GetAllQueriesByCustomer(Guid customerId, [FromQuery] PageParameters pageParameters)
        {
            var queries = _service.GetAllQueriesByCustomer(customerId,pageParameters);
            // Add pagination details in headers
            Response.Headers.Add("X-Total-Count", queries.TotalCount.ToString());
            Response.Headers.Add("X-Total-Pages", queries.TotalPages.ToString());
            Response.Headers.Add("X-Current-Page", queries.CurrentPage.ToString());
            Response.Headers.Add("X-Has-Next", queries.HasNext.ToString());
            Response.Headers.Add("X-Has-Previous", queries.HasPrevious.ToString());

            return Ok(queries);
        }

        [HttpGet, Authorize(Roles = "Admin,Employee")]
        public IActionResult GetAllPaginated([FromQuery] PageParameters pageParameters)
        {
            var queries = _service.GetPaginatedQueries(pageParameters);

            // Add pagination details in headers
            Response.Headers.Add("X-Total-Count", queries.TotalCount.ToString());
            Response.Headers.Add("X-Total-Pages", queries.TotalPages.ToString());
            Response.Headers.Add("X-Current-Page", queries.CurrentPage.ToString());
            Response.Headers.Add("X-Has-Next", queries.HasNext.ToString());
            Response.Headers.Add("X-Has-Previous", queries.HasPrevious.ToString());

            return Ok(queries);
        }
    }
}
