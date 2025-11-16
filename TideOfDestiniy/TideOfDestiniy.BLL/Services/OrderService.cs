using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TideOfDestiniy.BLL.DTOs.Responses;
using TideOfDestiniy.BLL.Interfaces;
using TideOfDestiniy.DAL.Interfaces;

namespace TideOfDestiniy.BLL.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepo _orderRepo;
        public OrderService(IOrderRepo orderRepo)
        {
            _orderRepo = orderRepo;
        }
        public async Task<IEnumerable<OrderHistoryDto>> GetOrderHistoryAsync(Guid userId)
        {
            var orders = await _orderRepo.GetByUserIdAsync(userId);

            // Map từ List<Order> (Entity) sang List<OrderHistoryDto> (DTO)
            return orders.Select(o => new OrderHistoryDto
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                Description = o.Description,
                Amount = o.Amount,
                Status = o.Status.ToString() // Chuyển enum thành string
            });
        }
    } 
}
