using Microsoft.AspNetCore.Mvc;
using TideOfDestiniy.BLL.Interfaces;

namespace TideOfDestiniy.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadController : ControllerBase
    {
        private readonly IUploadService _service;

        public UploadController(IUploadService service)
        {
            _service = service;
        }
        /// <summary>
        /// Upload file từ client lên server ASP.NET Core → đẩy tiếp lên R2
        /// </summary>
        [HttpPost("file")]
        [RequestSizeLimit(long.MaxValue)] // Cho phép file lớn
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { message = "Không có file nào được chọn." });

            // ✅ Kiểm tra định dạng file
            var extension = Path.GetExtension(file.FileName)?.ToLowerInvariant();
            if (extension != ".zip" && extension != ".rar")
                return BadRequest(new { message = "You can only upload .zip or .rar file." });

            try
            {
                using var stream = file.OpenReadStream();

                // Gọi service để upload lên Cloudflare R2
                var result = await _service.UploadToR2Async(stream, file.FileName, file.ContentType);

                return Ok(new
                {
                    message = "Upload thành công!",
                    file = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi upload file.", error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUploadedFileFromLocalDatabase()
        {
            var result = await _service.GetAllFilesAsync();
            return Ok(new
            {
                message = "Retrieve Success!",
                file = result
            });
        }
    }
}
