using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TideOfDestiniy.DAL.Context;
using TideOfDestiniy.DAL.Entities;
using TideOfDestiniy.DAL.Interfaces;

namespace TideOfDestiniy.DAL.Repositories
{
    public class UserRepo : IUserRepo
    {
        private TideOfDestinyDbContext _context;

        public UserRepo(TideOfDestinyDbContext context)
        {
            _context = context;
        }
        public Task<User?> GetUserByUsernameAsync(string username) => 
            _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Username == username);

        public async Task<User?> LoginAsync(string username, string password)
        {
            var user = await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
            {
                return null;
            }


            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return null;
            }

            return user;
        }

        public async Task<User?> ResgisterAsync(User user, string password, string comfirmPassword)
        {
            if(await _context.Users.AnyAsync(u => u.Username == user.Username || u.Email == user.Email))
            {
                return null; // Username or Email already exists
            }

            if(password != comfirmPassword)
            {
                return null; // Password and Confirm Password do not match
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            user.PasswordHash = passwordHash;
            user.UserRoles = new List<UserRole>
            {
                new UserRole
                {
                    RoleId = 2 // Mặc định gán RoleId = 2 (User)
                }
            };

            // 3. Thêm người dùng mới vào DbContext
            await _context.Users.AddAsync(user);

            // 4. Lưu thay đổi vào database
            await _context.SaveChangesAsync();

            return user;
        }
    }
}
