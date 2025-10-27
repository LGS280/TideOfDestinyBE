using Microsoft.EntityFrameworkCore;
using TideOfDestiniy.DAL.Context;
using TideOfDestiniy.DAL.Entities;
using TideOfDestiniy.DAL.Interfaces;

namespace TideOfDestiniy.DAL.Repositories
{
    public class OrderRepo : IOrderRepo
    {
        private readonly TideOfDestinyDbContext _context;

        public OrderRepo(TideOfDestinyDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
        }

        public Task UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
            return Task.CompletedTask;
        }

        public async Task<Order?> GetByPaymentOrderCodeAsync(int orderCode)
        {
            return await _context.Orders.FirstOrDefaultAsync(o => o.PaymentOrderCode == orderCode);
        }

        public async Task<bool> HasUserPurchasedGameAsync(Guid userId)
        {
            // Kiểm tra xem user đã có đơn hàng nào với trạng thái "Paid" chưa
            return await _context.Orders.AnyAsync(o => o.UserId == userId && o.Status == OrderStatus.Paid);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}