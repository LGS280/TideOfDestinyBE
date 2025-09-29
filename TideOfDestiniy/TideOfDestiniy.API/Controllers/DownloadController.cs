using Microsoft.AspNetCore.Mvc;
using TideOfDestiniy.BLL.Interfaces;

namespace TideOfDestiniy.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DownloadController : ControllerBase
    {
        private readonly IDownloadGameService _service;

        public DownloadController(IDownloadGameService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public IActionResult DownloadGame(int id)
        {
            var file = _service.GetFileById(id);
            if (file == null)
                return NotFound("Game not found in database");

            var filePath = _service.GetPhysicalPath(file);
            if (!System.IO.File.Exists(filePath))
                return NotFound("File not found on server");

            var contentType = "application/octet-stream"; // fallback type
            return PhysicalFile(filePath, contentType, file.FileName, enableRangeProcessing: true);
        }
    }
}
