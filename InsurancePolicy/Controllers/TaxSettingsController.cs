using InsurancePolicy.DTOs;
using InsurancePolicy.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace InsurancePolicy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaxSettingsController : ControllerBase
    {
        private readonly ITaxSettingsService _service;

        public TaxSettingsController(ITaxSettingsService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult Add(TaxSettingsRequestDto requestDto)
        {
            var id = _service.Add(requestDto);
            return Ok(id);
        }

        [HttpPut]
        public IActionResult Update(TaxSettingsRequestDto requestDto)
        {
            _service.Update(requestDto);
            return Ok(new { Message = "Updated successfully." });
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
