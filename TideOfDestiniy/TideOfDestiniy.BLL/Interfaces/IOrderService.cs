using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TideOfDestiniy.BLL.DTOs.Responses;

namespace TideOfDestiniy.BLL.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderHistoryDto>> GetOrderHistoryAsync(Guid userId);

    }
}
