using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TideOfDestiniy.DAL.Entities;

namespace TideOfDestiniy.DAL.Interfaces
{
    public interface IUserRepo
    {
        Task<User?> ResgisterAsync(User user, string password, string confirmPassword);
        Task<User?> LoginAsync(string username, string password);
        Task<List<User?>> GetUserAsync();
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByIdAsync(Guid id);
        Task<User> CreateUserAsync(User user);
        Task<bool> UpdateUserAsync(User user);


    }
}
