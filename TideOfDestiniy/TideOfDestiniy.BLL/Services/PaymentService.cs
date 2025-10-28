using Microsoft.Extensions.Configuration;
using Net.payOS;
using Net.payOS.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TideOfDestiniy.BLL.DTOs.Requests;
using TideOfDestiniy.BLL.Interfaces;
using TideOfDestiniy.DAL.Entities;
using TideOfDestiniy.DAL.Interfaces;
using TideOfDestiniy.DAL.Repositories;

namespace TideOfDestiniy.BLL.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly PayOS _payOS;
        private readonly IOrderRepo _orderRepo;
        private readonly IUserRepo _userRepo;
        private const decimal GamePrice = 5000; // Đặt giá game cố định ở đây (ví dụ: 250,000 VND)
        private const string GameDescription = "Tide of Destiny";

        public PaymentService(IConfiguration configuration, IOrderRepo orderRepo, IUserRepo userRepo)
        {
            var clientId = configuration["PayOsSettings:ClientId"];
            var apiKey = configuration["PayOsSettings:ApiKey"];
            var checksumKey = configuration["PayOsSettings:ChecksumKey"];
            _payOS = new PayOS(clientId, apiKey, checksumKey);
            _orderRepo = orderRepo;
            _userRepo = userRepo;
        }
        public async Task<string> CreatePaymentLink(Guid userId, string returnUrl, string cancelUrl)
        {
            if (await _orderRepo.HasUserPurchasedGameAsync(userId))
            {
                throw new InvalidOperationException("You have already purchased this game.");
            }

            int orderCode = (int)DateTimeOffset.Now.ToUnixTimeSeconds();
            var newOrder = new Order
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Status = OrderStatus.Pending,
                Amount = GamePrice,
                Description = GameDescription,
                PaymentOrderCode = orderCode
            };

            await _orderRepo.AddAsync(newOrder);
            await _orderRepo.SaveChangesAsync();

            // 3. Tạo request cho PayOS
            List<ItemData> items = new List<ItemData>();
            // Nếu bạn muốn thêm item cụ thể (không cần thiết với kịch bản 1 game)
            // items.Add(new ItemData("Tide of Destiny", 1, (int)GamePrice));

            var paymentData = new PaymentData(
                orderCode,
                (int)newOrder.Amount,
                newOrder.Description,
                items, // <<== Phải truyền danh sách item vào
                cancelUrl,
                returnUrl
            );

            CreatePaymentResult createPaymentResult = await _payOS.createPaymentLink(paymentData);

            newOrder.PaymentLinkId = createPaymentResult.paymentLinkId;
            await _orderRepo.UpdateAsync(newOrder);
            await _orderRepo.SaveChangesAsync();

            return createPaymentResult.checkoutUrl;


        }

        public async Task HandleWebhook(WebhookData webhookData)
        {
            // Thư viện mới có hàm verify riêng, nhưng chúng ta sẽ xử lý trực tiếp
            // vì webhookData đã được parse từ body request

            long orderCode = webhookData.orderCode;
            var order = await _orderRepo.GetByPaymentOrderCodeAsync((int)(orderCode % int.MaxValue));

            if (order == null || order.Status != OrderStatus.Pending)
            {
                return;
            }

            // Dựa trên tài liệu, webhook không có trường 'status' rõ ràng.
            // Chúng ta sẽ giả định webhook chỉ được gọi khi thanh toán thành công.
            // Để chắc chắn, bạn nên kiểm tra trong dashboard của PayOS
            // xem webhook gửi về có code thành công là gì (ví dụ code = "00").
            if (webhookData.code == "00") // "00" thường là mã giao dịch thành công
            {
                order.Status = OrderStatus.Paid;
                var user = await _userRepo.GetUserByIdAsync(order.UserId);
                if (user != null)
                {
                    user.HasPurchasedGame = true;
                    await _userRepo.UpdateUserAsync(user);
                }
            }
            else // Các mã lỗi khác
            {
                order.Status = OrderStatus.Failed;
            }

            await _orderRepo.UpdateAsync(order);
            await _orderRepo.SaveChangesAsync();
        }
    }
}
