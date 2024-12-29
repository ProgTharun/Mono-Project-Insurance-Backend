using InsurancePolicy.DTOs;
using InsurancePolicy.Services;
using InsurancePolicy.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace InsurancePolicy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstallmentController : ControllerBase
    {
        private readonly IInstallmentService _installmentService;

        public InstallmentController(IInstallmentService installmentService)
        {
            _installmentService = installmentService;
        }

        [HttpPost]
        public IActionResult AddInstallment(InstallmentRequestDto installmentDto)
        {
            
            var installmentId = _installmentService.AddInstallment(installmentDto);
            return Ok(new { InstallmentId = installmentId });
        }

        [HttpPut]
        public IActionResult UpdateInstallment(InstallmentRequestDto installmentDto)
        {
            _installmentService.UpdateInstallment(installmentDto);
            return Ok(new { Message = "Installment updated successfully." });
        }

        [HttpGet("{installmentId}")]
        public IActionResult GetInstallmentById(Guid installmentId)
        {
            var installment = _installmentService.GetInstallmentById(installmentId);
            return Ok(installment);
        }

        [HttpGet("policy/{policyId}")]
        public IActionResult GetPaginatedInstallmentsForPolicy(Guid policyId, [FromQuery] PageParameters pageParameters)
        {
            var installments = _installmentService.GetPaginatedInstallmentsForPolicy(policyId, pageParameters);

            // Add pagination metadata to headers
            Response.Headers.Add("X-Total-Count", installments.TotalCount.ToString());
            Response.Headers.Add("X-Page-Size", installments.PageSize.ToString());
            Response.Headers.Add("X-Current-Page", installments.CurrentPage.ToString());
            Response.Headers.Add("X-Total-Pages", installments.TotalPages.ToString());
            Response.Headers.Add("X-Has-Next", installments.HasNext.ToString());
            Response.Headers.Add("X-Has-Previous", installments.HasPrevious.ToString());

            return Ok(installments);
        }

        [HttpDelete("{installmentId}")]
        public IActionResult DeleteInstallment(Guid installmentId)
        {
            _installmentService.DeleteInstallment(installmentId);
            return Ok(new { Message = "Deleted Successfully!" });
        }
    }
}
