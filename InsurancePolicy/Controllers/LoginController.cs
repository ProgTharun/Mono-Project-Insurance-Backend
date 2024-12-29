using InsurancePolicy.Data;
using InsurancePolicy.DTOs;
using InsurancePolicy.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Claim = System.Security.Claims.Claim;
using System.Text;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;

namespace InsurancePolicy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        public LoginController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        [HttpPost]
        public IActionResult Login(LoginDto loginDto)
        {
            var existingUser = _context.Users
                .Include(user => user.Role)
                .Include(user => user.Customer)
                .Include(user => user.Agent)
                .Include(user => user.Employee)
                .Include(user => user.Admin)
                .FirstOrDefault(a => a.UserName == loginDto.UserName);

            if (existingUser != null)
            {
                if (BCrypt.Net.BCrypt.Verify(loginDto.Password, existingUser.Password))
                {
                    // Check if the user is active based on their role and status
                    bool isActive = existingUser.Role.Name switch
                    {
                        "Admin" => existingUser.Admin != null && existingUser.Admin.Status,
                        "Customer" => existingUser.Customer != null && existingUser.Customer.Status,
                        "Agent" => existingUser.Agent != null && existingUser.Agent.Status,
                        "Employee" => existingUser.Employee != null && existingUser.Employee.Status,
                        _ => false // Default to inactive for undefined roles
                    };

                    if (!isActive)
                    {
                        return BadRequest("User is not active.");
                    }

                    // Generate the token if the user is active
                    var token = CreateToken(existingUser);
                    Response.Headers.Add("Jwt", token);

                    if (existingUser.Role.Name == "Customer")
                    {
                        return Ok(new { roleName = existingUser.Role.Name, customerId = existingUser.Customer.CustomerId });
                    }
                    else if (existingUser.Role.Name == "Admin")
                    {
                        return Ok(new { roleName = existingUser.Role.Name, adminId = existingUser.Admin.AdminId });
                    }
                    else if (existingUser.Role.Name == "Agent")
                    {
                        return Ok(new { roleName = existingUser.Role.Name, agentId = existingUser.Agent.AgentId });
                    }
                    else if (existingUser.Role.Name == "Employee")
                    {
                        return Ok(new { roleName = existingUser.Role.Name, employeeId = existingUser.Employee.EmployeeId});
                    }

                    return Ok(new { roleName = existingUser.Role.Name });
                }
            }

            // If the username/password doesn't match or the user is not valid
            return BadRequest("Invalid username/password or user is not active.");
        }
        private string CreateToken(User user)
        {
            var roleName = user.Role.Name;
            List<Claim> claims = new List<Claim>()
     {
         new Claim(ClaimTypes.Name,user.UserName),
         new Claim(ClaimTypes.Role,roleName),
     };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Key").Value));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            //token construction
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred
                );
            //generate the token
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
        [HttpPost("ChangePassword")]
        public IActionResult ChangePassword([FromBody] PasswordChangeDto changePasswordDto)
        {
            // Extract the username of the logged-in user from the token
            var usernameFromToken = User.Identity?.Name;
            if (string.IsNullOrEmpty(usernameFromToken))
            {
                return Unauthorized(new { Message = "Invalid token or user is not authenticated." });
            }

            // Ensure the username in the token matches the username in the request
            if (!usernameFromToken.Equals(changePasswordDto.UserName, StringComparison.OrdinalIgnoreCase))
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { Message = "You can only change your own password." });
            }

            // Find the user by their username
            var existingUser = _context.Users.FirstOrDefault(u => u.UserName == changePasswordDto.UserName);
            if (existingUser == null)
            {
                return BadRequest(new { Message = "User not found." });
            }

            // Verify the old password
            if (!BCrypt.Net.BCrypt.Verify(changePasswordDto.OldPassword, existingUser.Password))
            {
                return BadRequest(new { Message = "Old password is incorrect." });
            }

            // Hash the new password
            var newHashedPassword = BCrypt.Net.BCrypt.HashPassword(changePasswordDto.NewPassword);

            // Overwrite the user's password with the new hash
            existingUser.Password = newHashedPassword;

            // Save changes to the database
            _context.Users.Update(existingUser);
            _context.SaveChanges();

            return Ok(new { Message = "Password changed successfully." });
        }
    }
}
