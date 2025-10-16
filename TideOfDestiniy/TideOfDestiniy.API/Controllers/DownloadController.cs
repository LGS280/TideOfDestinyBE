using Amazon.S3;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using TideOfDestiniy.BLL.Interfaces;
using TideOfDestiniy.DAL.Interfaces;

namespace TideOfDestiniy.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DownloadController : ControllerBase
    {
        private readonly IDownloadGameService _service;
        private readonly IR2StorageService _storageService;
        private readonly IFileRepo _repo;
        public DownloadController(IDownloadGameService service, IR2StorageService storageService, IFileRepo repo)
        {
            _service = service;
            _storageService = storageService;
            _repo = repo;
        }

        [HttpGet("download/{fileName}")]
        public async Task<IActionResult> Download(string fileName)
        {
            try
            {
                var fileStream = await _storageService.DownloadFileAsync(fileName);
                
                // Try to get file info from database for better content type
                var fileInfo = await _repo.GetByFileNameAsync(fileName);
                var contentType = fileInfo?.ContentType ?? "application/octet-stream";
                
                return File(fileStream, contentType, fileName);
            }
            catch (AmazonS3Exception ex)
            {
                return NotFound($"File not found or error accessing R2: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("download-by-id/{id}")]
        public async Task<IActionResult> DownloadById(int id)
        {
            try
            {
                var fileInfo = await _service.GetByIdAsync(id);
                if (fileInfo == null)
                {
                    return NotFound("File not found in database");
                }

                var fileStream = await _storageService.DownloadFileAsync(fileInfo.FileName);
                return File(fileStream, fileInfo.ContentType, fileInfo.FileName);
            }
            catch (AmazonS3Exception ex)
            {
                return NotFound($"File not found or error accessing R2: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("download-by-key/{key}")]
        public async Task<IActionResult> DownloadByKey(string key)
        {
            try
            {
                var fileStream = await _storageService.DownloadFileByKeyAsync(key);
                var fileName = Path.GetFileName(key);
                return File(fileStream, "application/octet-stream", fileName);
            }
            catch (AmazonS3Exception ex)
            {
                return NotFound($"File not found or error accessing R2: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("donwload-lastest-file")]
        public async Task<IActionResult> DownloadlastestFile()
        {
            try
            {
                var lastestFile = await _service.GetLastestFileAsync();
                if (lastestFile == null)
                    return NotFound("File not found in database");
                var fileStream = await _storageService.DownloadFileAsync(lastestFile.FileName);
                return File(fileStream, lastestFile.ContentType, lastestFile.FileName);
            }
            catch (AmazonS3Exception ex)
            {
                return NotFound($"File not found or error accessing R2: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
