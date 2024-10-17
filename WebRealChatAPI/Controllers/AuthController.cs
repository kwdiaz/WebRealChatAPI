using Microsoft.AspNetCore.Mvc;
using WebRealChatAPI.Services;
using WebRealChatAPI.Models;

namespace WebRealChatAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        // Reference to the authentication service
        private readonly IAuthService _authService;

        // Constructor to inject the authentication service
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // Endpoint to handle user registration
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequestModel request)
        {
            var result = _authService.Register(request);
            if (result.Success)
            {
                return Ok(result.Message); // Return success message if registration is successful
            }
            return BadRequest(result.Message); // Return error message if registration fails
        }

        // Endpoint to handle user login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestModel request)
        {
            var result = _authService.Login(request);
            if (result.Success)
            {
                return Ok(new { token = result.Token }); // Return token if login is successful
            }
            return Unauthorized(new { message = result.Message }); // Return error message if login fails
        }
    }
}
