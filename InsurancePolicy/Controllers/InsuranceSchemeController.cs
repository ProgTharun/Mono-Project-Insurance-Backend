using InsurancePolicy.DTOs;
using InsurancePolicy.Helpers;
using InsurancePolicy.Models;
using InsurancePolicy.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace InsurancePolicy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InsuranceSchemeController : ControllerBase
    {
        private readonly IInsuranceSchemeService _service;

        public InsuranceSchemeController(IInsuranceSchemeService service)
        {
            _service = service;
        }
        [HttpGet("count")]
        public IActionResult GetCount()
        {
            var count=_service.GetSchemeCount();
            return Ok(new { count });   
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] PageParameters pageParameters)
        {
            var schemes = _service.GetAllPaginated(pageParameters);
            Response.Headers.Add("X-Current-Page", schemes.CurrentPage.ToString());
            Response.Headers.Add("X-Total-Pages", schemes.TotalPages.ToString());
            Response.Headers.Add("X-Has-Next", schemes.HasNext.ToString());
            Response.Headers.Add("X-Has-Previous", schemes.HasPrevious.ToString());
            Response.Headers.Add("X-Total-Count", schemes.TotalCount.ToString());
            return Ok(schemes);
        }

        [HttpGet("Plan/{planId}"), Authorize(Roles = "Admin,Agent,Customer")]
        public IActionResult GetAllByPlanId(Guid planId, [FromQuery] PageParameters pageParameters)
        {
            var schemes = _service.GetAllByPlanIdPaginated(planId, pageParameters);
            Response.Headers.Add("X-Current-Page", schemes.CurrentPage.ToString());
            Response.Headers.Add("X-Total-Pages", schemes.TotalPages.ToString());
            Response.Headers.Add("X-Has-Next", schemes.HasNext.ToString());
            Response.Headers.Add("X-Has-Previous", schemes.HasPrevious.ToString());
            Response.Headers.Add("X-Total-Count", schemes.TotalCount.ToString());
            return Ok(schemes);
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var scheme = _service.GetById(id);
            return Ok(scheme);
        }

        [HttpPost, Authorize(Roles = "Admin")]
        public IActionResult Add(InsuranceSchemeRequestDto scheme)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join("; ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                throw new ValidationException($"{errors}");
            }
            var newScheme = _service.Add(scheme);
            return Ok(newScheme);
        }

        [HttpPut, Authorize(Roles = "Admin")]
        public IActionResult Modify(InsuranceSchemeRequestDto scheme)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join("; ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                throw new ValidationException($"{errors}");
            }
            _service.Update(scheme);
            return Ok(scheme);
        }

        [HttpGet("{schemeId}/customer/{customerId}/exists")]
        public IActionResult IsCustomerAssociatedWithScheme(Guid schemeId, Guid customerId)
        {
            var isAssociated = _service.IsCustomerAssociatedWithScheme(schemeId, customerId);
            return Ok(new { IsAssociated = isAssociated });
        }
        [HttpPut("activate"), Authorize(Roles = "Admin")]
        public IActionResult Activate(Guid id)
        {
            _service.Activate(id);
            return Ok(id);
        }

        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public IActionResult Delete(Guid id)
        {
            _service.Delete(id);
            return Ok(new { Message = "Deleted Successfully!" });
        }
    }
}
