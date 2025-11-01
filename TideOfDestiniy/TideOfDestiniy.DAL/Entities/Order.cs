using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TideOfDestiniy.DAL.Entities
{
    public enum OrderStatus
    {
        Pending,    // Đang chờ thanh toán
        Paid,       // Đã thanh toán thành công
        Cancelled,  // Đã hủy
        Failed      // Thanh toán thất bại
    }

    public class Order
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }
        public virtual User User { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; } // Số tiền thanh toán

        [MaxLength(200)]
        public string Description { get; set; } // Mô tả, ví dụ: "Purchase Tide of Destiny Game"

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        // Thông tin từ PayOS để đối soát
        public int PaymentOrderCode { get; set; } // OrderCode duy nhất của PayOS
        public string? PaymentLinkId { get; set; } // Link thanh toán của PayOS
    }
}