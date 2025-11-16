using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TideOfDestiniy.BLL.DTOs.Responses
{
    public class OrderHistoryDto
    {
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string Description { get; set; } // Tên sản phẩm đã mua
        public decimal Amount { get; set; }
        public string Status { get; set; } // Trả về dạng chuỗi cho FE dễ hiển thị (e.g., "Paid", "Cancelled")
    }
}
