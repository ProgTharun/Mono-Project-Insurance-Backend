using InsurancePolicy.DTOs;
using InsurancePolicy.Models;
using InsurancePolicy.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace InsurancePolicy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _service.GetAll();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var user = _service.GetById(id);
            return Ok(user);
        }

        [HttpPost]
        public IActionResult Add(User user)
        {
            var newUser = _service.Add(user);
            return Ok(newUser);
        }


        [HttpPut]
        public IActionResult Modify(User user)
        {
            _service.Update(user);
            return Ok(user);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _service.Delete(id);
            return Ok(new { Message = "Deleted Successfully!" });
        }
    }
}
