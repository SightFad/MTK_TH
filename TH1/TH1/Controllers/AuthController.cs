using Microsoft.AspNetCore.Mvc;
using TH1.DTOs;
using TH1.Services;
using TH1.Patterns.Singleton;

namespace TH1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
            LoggerService.Instance.Log("AuthController initialized.");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            try
            {
                var user = await _authService.Register(registerDto);
                return Ok(new { user.UserId, user.Username, user.Email });
            }
            catch (Exception ex)
            {
                LoggerService.Instance.Log($"Registration failed: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            try
            {
                var token = await _authService.Login(loginDto);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                LoggerService.Instance.Log($"Login failed: {ex.Message}");
                return Unauthorized(ex.Message);
            }
        }
    }
}
