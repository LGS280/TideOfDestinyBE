using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TideOfDestiniy.BLL.DTOs;
using TideOfDestiniy.DAL.Entities;

namespace TideOfDestiniy.BLL.Interfaces
{
    public interface IUserService
    {
        Task<AuthResultDTO> RegisterAsync(RegisterDTO register);
        Task<AuthResultDTO> LoginAsync(LoginDTO loginDTO);
        Task<bool> UserExistsAsync(string username);
        Task<List<UserDTO>?> GetUserAsync();
    }
}
