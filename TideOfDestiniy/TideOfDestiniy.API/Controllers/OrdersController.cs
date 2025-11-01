using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TideOfDestiniy.BLL.Interfaces;

namespace TideOfDestiniy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetMyOrderHistory()
        {
            // Lấy User ID từ token xác thực để đảm bảo user chỉ thấy lịch sử của chính mình
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdString, out var userId))
            {
                return Unauthorized("Invalid user token.");
            }

            try
            {
                var history = await _orderService.GetOrderHistoryAsync(userId);
                return Ok(history);
            }
            catch (Exception ex)
            {
                // Log lỗi
                return StatusCode(500, "An error occurred while fetching your order history.");
            }
        }

    }
}
