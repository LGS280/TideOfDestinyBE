using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TideOfDestiniy.DAL.Entities
{
    public class User
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        public string? PasswordHash { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLoginAt { get; set; }

        // Navigation Properties (Quan hệ)
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public bool EmailConfirmed { get; set; }
        //public ICollection<SupportTicket> SupportTickets { get; set; } = new List<SupportTicket>();

    }
}
