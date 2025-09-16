using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TideOfDestiniy.BLL.DTOs;
using TideOfDestiniy.BLL.Interfaces;
using TideOfDestiniy.BLL.Services;

namespace TideOfDestiniy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.RegisterAsync(registerDto);

            if (!result.Succeeded)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(new { message = result.Message });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.LoginAsync(loginDto);

            if (!result.Succeeded)
            {
                return Unauthorized(new { message = result.Message });
            }

            // Trả về token cho client
            return Ok(new { message = "Login Successful" ,token = result.Token });
        }

        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginDTO googleLoginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await (_userService as Authorization)?.LoginWithGoogleAsync(googleLoginDto);
            if (result == null || !result.Succeeded)
            {
                return Unauthorized(new { message = result?.Message ?? "Google login failed." });
            }
            return Ok(new { message = "Google login successful.", token = result.Token });
        }
    }
}
