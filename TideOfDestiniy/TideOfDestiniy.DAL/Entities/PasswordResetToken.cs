using System;
using System.ComponentModel.DataAnnotations;

namespace TideOfDestiniy.DAL.Entities
{
    public class PasswordResetToken
    {
        public Guid Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        public string Token { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime ExpiresAt { get; set; }
        
        public bool IsUsed { get; set; } = false;
    }
}
