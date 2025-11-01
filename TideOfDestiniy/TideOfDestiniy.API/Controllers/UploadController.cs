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
        [RequestSizeLimit(long.MaxValue)] // cho phép file lớn
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Không có file nào được chọn");

            // Đọc file stream
            using var stream = file.OpenReadStream();

            // Gọi service để upload lên Cloudflare R2
            var result = await _service.UploadToR2Async(stream, file.FileName, file.ContentType);

            return Ok(new
            {
                message = "Upload thành công!",
                file = result
            });
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
