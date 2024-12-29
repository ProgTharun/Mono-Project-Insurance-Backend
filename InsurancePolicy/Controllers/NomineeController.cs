using InsurancePolicy.DTOs;
using InsurancePolicy.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace InsurancePolicy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NomineeController : ControllerBase
    {
        private readonly INomineeService _nomineeService;

        public NomineeController(INomineeService nomineeService)
        {
            _nomineeService = nomineeService;
        }

        [HttpPost]
        public IActionResult AddNominee(NomineeRequestDto nomineeDto)
        {
           var nomineeId = _nomineeService.AddNominee(nomineeDto);
            return Ok(new { NomineeId = nomineeId });
        }

        [HttpPut]
        public IActionResult UpdateNominee(NomineeRequestDto nomineeDto)
        {
            _nomineeService.UpdateNominee(nomineeDto);
            return Ok(new { Message = "Nominee updated successfully." });
        }

        [HttpGet("{nomineeId}")]
        public IActionResult GetNomineeById(Guid nomineeId)
        {
            var nominee = _nomineeService.GetNomineeById(nomineeId);
            return Ok(nominee);
        }

        [HttpGet("policy/{policyId}")]
        public IActionResult GetAllNomineesForPolicy(Guid policyId)
        {

            var nominees = _nomineeService.GetAllNomineesForPolicy(policyId);
            return Ok(nominees);
        }

        [HttpDelete("{nomineeId}")]
        public IActionResult DeleteNominee(Guid nomineeId)
        {
            _nomineeService.DeleteNominee(nomineeId);
            return Ok(new { Message = "Deleted Successfully!" });
        }
    }
}
