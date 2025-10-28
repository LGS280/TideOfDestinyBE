using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Net.payOS.Types;
using System.Security.Claims;
using TideOfDestiniy.BLL.DTOs.Requests;
using TideOfDestiniy.BLL.Interfaces;
using TideOfDestiniy.DAL.Interfaces;

namespace TideOfDestiniy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IOrderRepo _orderRepo; // Thêm repo để kiểm tra

        public PaymentController(IPaymentService paymentService, IOrderRepo orderRepo)
        {
            _paymentService = paymentService;
            _orderRepo = orderRepo;
        }

        [HttpPost("create-payment-link")]
        [Authorize]
        public async Task<IActionResult> CreatePaymentLink([FromBody] CreatePaymentRequestDto requestDto)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdString, out var userId))
            {
                return Unauthorized("Invalid user token.");
            }

            try
            {
                var checkoutUrl = await _paymentService.CreatePaymentLink(userId, requestDto.ReturnUrl, requestDto.CancelUrl);
                return Ok(new { CheckoutUrl = checkoutUrl });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message }); // Trả về 409 Conflict nếu đã mua
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("purchase-status")]
        [Authorize]
        public async Task<IActionResult> GetPurchaseStatus()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdString, out var userId))
            {
                return Unauthorized("Invalid user token.");
            }

            var hasPurchased = await _orderRepo.HasUserPurchasedGameAsync(userId);
            return Ok(new { HasPurchased = hasPurchased });
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> HandlePayOsWebhook([FromBody] WebhookData webhookData)
        {
            try
            {
                await _paymentService.HandleWebhook(webhookData);
                return Ok();
            }
            catch
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("confirm")]
        [Authorize]
        public async Task<IActionResult> ConfirmPayment([FromBody] long orderCode)
        {
            try
            {
                // ✅ Gọi service xử lý xác nhận thanh toán
                var result = await _paymentService.ConfirmPayment(orderCode);

                if (!result)
                    return BadRequest(new { success = false, message = "Payment not completed yet." });

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

    }
}
