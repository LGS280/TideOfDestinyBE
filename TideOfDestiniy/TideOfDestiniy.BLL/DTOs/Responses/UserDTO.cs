using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TideOfDestiniy.BLL.DTOs.Responses
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;
        //public DateTime CreatedAt { get; set; }
        //public DateTime UpdatedAt { get; set; }
    }
}
