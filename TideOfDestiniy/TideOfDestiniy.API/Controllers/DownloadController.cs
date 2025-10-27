using Amazon.S3;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Security.Claims;
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
        private readonly IFileRepo _fileRepo;
        private readonly IUserService _userService; // <<== THÊM DÒNG NÀY

        // Cập nhật Constructor
        public DownloadController(
            IDownloadGameService service,
            IR2StorageService storageService,
            IFileRepo fileRepo,
            IUserService userService) // <<== THÊM THAM SỐ NÀY
        {
            _service = service;
            _storageService = storageService;
            _fileRepo = fileRepo;
            _userService = userService; // <<== THÊM DÒNG NÀY
        }


        [HttpGet("download-latest-game")] // Đổi tên cho rõ ràng
        [Authorize] // Yêu cầu người dùng phải đăng nhập
        public async Task<IActionResult> DownloadLatestGame()
        {
            // 1. Lấy User ID từ token xác thực
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdString, out var userId))
            {
                return Unauthorized("Invalid user token. Please log in again.");
            }

            // 2. Kiểm tra trong database xem người dùng đã mua game chưa
            var user = await _userService.GetUserAsync(userId);
            if (user == null)
            {
                // Trường hợp hiếm gặp, user có token hợp lệ nhưng không có trong DB
                return NotFound("User associated with this token was not found.");
            }

            if (!user.HasPurchasedGame)
            {
                // Nếu chưa mua, trả về lỗi 403 Forbidden
                return Forbid("Access denied. You have not purchased the game. Please complete the payment to download.");
            }

            // 3. Nếu đã mua, tiến hành tải file mới nhất
            try
            {
                // Lưu ý: phương thức này của bạn trả về byte array, có thể gây tốn RAM nếu file lớn.
                // Nếu có thể, hãy chỉnh sửa `DownloadLatestFileAsync` để trả về Stream.
                var (fileBytes, fileName) = await _storageService.DownloadLatestFileAsync();

                if (fileBytes == null || fileBytes.Length == 0)
                {
                    return NotFound("The latest game build is not available for download at the moment.");
                }

                // Trả về file cho người dùng
                return File(fileBytes, "application/zip", fileName); // "application/zip" hoặc "application/x-zip-compressed"
            }
            catch (AmazonS3Exception ex)
            {
                // Log lỗi chi tiết (không trả về cho client)
                return NotFound($"Could not retrieve the game file from storage. Please try again later. Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Log lỗi chi tiết
                return StatusCode(500, $"An internal server error occurred while processing your download request. Error: {ex.Message}");
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
                var (fileBytes, fileName) = await _storageService.DownloadLatestFileAsync();
                return File(fileBytes, "application/x-zip-compressed", fileName);
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
