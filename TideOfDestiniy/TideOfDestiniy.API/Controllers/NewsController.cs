using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TideOfDestiniy.BLL.DTOs;
using TideOfDestiniy.BLL.Interfaces;
using TideOfDestiniy.BLL.Services;

namespace TideOfDestiniy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class NewsController : ControllerBase
    {
        private INewsService _newsService;

        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNews()
        {
            var newsList = await _newsService.GetAllNewsAsync();
            return Ok(newsList);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNewsById(Guid id)
        {
            var news = await _newsService.GetNewsById(id);
            if (news == null)
            {
                return NotFound(new { message = "News not found." });
            }
            return Ok(news);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateNews([FromBody] CreateNewsDTO newsDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }




            var userIdString =  User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Kiểm tra và chuyển đổi chuỗi sang Guid
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            {
                // Token không hợp lệ hoặc không chứa User ID dạng Guid
                return Unauthorized("Invalid token: User ID is missing or invalid.");
            }
            // =======================================

            var result = await _newsService.CreateNewsAsync(newsDTO, userId);
            if (!result.Succeeded)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(new { message = result.Message });
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateNews(Guid id, [FromBody] UpdateNewsDTO newsDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _newsService.UpdateNewsAsync(newsDTO, id);
            if (!result.Succeeded)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(new { message = result.Message });
        }
    }
}
