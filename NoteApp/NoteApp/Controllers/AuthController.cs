using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NoteApp.Models;
using NoteApp.Services;

namespace NoteApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserLogin req)
        {
            try
            {
                var user = await _authService.RegisterAsync(req.Username, req.Password);
                if (user == null)
                    return NotFound("User already exists!!!");

                return Ok(new { Id = user.id, Username = user.user_name });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLogin req)
        {
            try
            {
                var token = await _authService.LoginAsync(req.Username, req.Password);
                if (token == null)
                    return NotFound("User already exists!!!");

                return Ok(new { Token = token });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }
}
