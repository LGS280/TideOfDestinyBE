using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TideOfDestiniy.BLL.Interfaces;

namespace TideOfDestiniy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly IPhotoService _photoService;

        public PhotosController(IPhotoService photoService)
        {
            _photoService = photoService;
        }

        [HttpPost("upload-news-image")]
        [Authorize(Roles = "Admin")] // Chỉ Admin mới được upload ảnh cho tin tức
        public async Task<IActionResult> UploadNewsImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var result = await _photoService.AddPhotoAsync(file);

            if (!result.Succeeded)
            {
                return BadRequest(result.ErrorMessage);
            }

            // Trả về URL của ảnh đã upload
            // Frontend sẽ lấy URL này để gán vào ImageUrl khi tạo/cập nhật News
            return Ok(new { url = result.Url });
        }
    }
}
