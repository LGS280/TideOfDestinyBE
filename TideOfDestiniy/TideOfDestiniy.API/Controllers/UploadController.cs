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

        [HttpPost("game")]
        public async Task<IActionResult> UploadGame(IFormFile file)
        {
            try
            {
                var result = await _service.SaveFileAsync(file);
                return Ok(new { file = result.FileName, url = result.FilePath, size = result.FileSize });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
