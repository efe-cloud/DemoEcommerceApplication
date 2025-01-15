using Ecommerce.library.Responses;
using ECommerce.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.library.Models;
using Microsoft.AspNetCore.Authorization;

namespace ECommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ServiceResponse>> Register([FromBody] UserRegisterDto dto)
        {
            var result = await _authService.RegisterAsync(dto.Email, dto.Password);
            if (!result.Success) return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] UserLoginDto dto)
        {
            var token = await _authService.LoginAsync(dto.Email, dto.Password);
            if (token == null) return Unauthorized("Invalid credentials");

            return Ok(token);
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<User>> GetCurrentUser()
        {
            // We can read email from the claims
            var email = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
                return NotFound("User not found in token claims.");

            var user = await _authService.GetUserByEmailAsync(email);
            if (user == null) return NotFound("User does not exist in DB.");

            // Return the user but hide the password fields
            return new User
            {
                Id = user.Id,
                Email = user.Email
            };
        }
    }

    // Dtos for register/login
    public class UserRegisterDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class UserLoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
