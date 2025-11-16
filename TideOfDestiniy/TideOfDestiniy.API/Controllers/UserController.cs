using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TideOfDestiniy.BLL.DTOs.Requests;
using TideOfDestiniy.BLL.DTOs.Responses;
using TideOfDestiniy.BLL.Interfaces;
using TideOfDestiniy.BLL.Services;

namespace TideOfDestiniy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _userService;
        private IPasswordResetService _passwordResetService;

        public UserController(IUserService userService, IPasswordResetService passwordResetService)
        {
            _userService = userService;
            _passwordResetService = passwordResetService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetUserAsync();
            return Ok(users);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO forgotPasswordDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _passwordResetService.ForgotPasswordAsync(forgotPasswordDto);

            if (!result.Succeeded)
            {
                return BadRequest(new { message = result.Message });
            }

            // Don't return token in response for security - it's sent via email
            return Ok(new { message = result.Message });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDTO resetPasswordDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _passwordResetService.ResetPasswordAsync(resetPasswordDto);

            if (!result.Succeeded)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(new { message = result.Message });
        }
    }
}
