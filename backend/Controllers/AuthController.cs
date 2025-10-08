using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentScoreTracker.API.Data;
using StudentScoreTracker.API.Models;

namespace AuthBackend.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request?.Email) || string.IsNullOrEmpty(request?.Password))
                    return BadRequest(new { message = "Email and password are required" });

                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == request.Email && u.Password == request.Password);

                if (user == null)
                    return Unauthorized(new { message = "Invalid email or password" });

                // For demo only: returns dummy token
                return Ok(new { token = "dummy-jwt-token" });
            }
            catch (Exception ex)
            {
                // Log exception here or debug
                return StatusCode(500, new { message = "Internal server error", detail = ex.Message });
            }
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
