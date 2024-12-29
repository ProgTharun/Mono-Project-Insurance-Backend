using InsurancePolicy.DTOs;
using InsurancePolicy.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace InsurancePolicy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _service;

        public AdminController(IAdminService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var admins = _service.GetAll();
            return Ok(admins);
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var admin = _service.GetById(id);
            return Ok(admin);
        }
        [HttpGet("ByName/{name}")]
        public IActionResult GetByName(string name)
        {
            var admin = _service.GetByName(name);
            return Ok(admin);
        }


        [HttpPost]
        public IActionResult Add(AdminRequestDto adminRequestDto)
        {
            var newAdminId = _service.Add(adminRequestDto);
            return Ok(new { AdminId = newAdminId, Message = "Admin added successfully" });
        }

        [HttpPut]
        public IActionResult Modify(AdminRequestDto adminRequestDto)
        {
            _service.Update(adminRequestDto);
            return Ok(new { Message = "Admin updated successfully" });
        }
        [HttpPut("Profile")]
        public IActionResult UpdateUsername(AdminRequestDto adminRequestDto)
        {
            // Call the Update method in the service with the full DTO
            _service.UpdateUsername(adminRequestDto);

            return Ok(new { Message = "Username updated successfully." });
        }
    }
}
