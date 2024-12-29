using InsurancePolicy.DTOs;
using InsurancePolicy.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace InsurancePolicy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InsuranceSettingsController : ControllerBase
    {
        private readonly IInsuranceSettingsService _service;

        public InsuranceSettingsController(IInsuranceSettingsService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult Add(InsuranceSettingsRequestDto requestDto)
        {
            var id = _service.Add(requestDto);
            return Ok(id);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var settings = _service.GetById(id);
            return Ok(settings);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var settings = _service.GetAll();
            return Ok(settings);
        }
    }
}
