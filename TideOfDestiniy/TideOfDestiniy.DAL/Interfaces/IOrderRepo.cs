using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TideOfDestiniy.DAL.Entities;

namespace TideOfDestiniy.DAL.Interfaces
{
    public interface IOrderRepo
    {
        Task AddAsync(Order order);
        Task UpdateAsync(Order order);
        Task<Order?> GetByPaymentOrderCodeAsync(int orderCode);
        Task<bool> HasUserPurchasedGameAsync(Guid userId);
        Task<int> SaveChangesAsync();

    }
}
