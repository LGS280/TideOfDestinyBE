// File: Entities/Product.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TideOfDestiniy.DAL.Entities
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } // Ví dụ: "Tide of Destiny - Full Game"

        [MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        public bool IsActive { get; set; } = true;

        // Không còn liên kết đến GameBuild nữa
        // Bỏ quan hệ với OrderItem vì một đơn hàng chỉ có 1 item này
    }
}