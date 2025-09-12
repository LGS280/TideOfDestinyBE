using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TideOfDestiniy.BLL.DTOs;
using TideOfDestiniy.BLL.Interfaces;
using TideOfDestiniy.DAL.Entities;
using TideOfDestiniy.DAL.Interfaces;
using TideOfDestiniy.DAL.Repositories;

namespace TideOfDestiniy.BLL.Services
{
    public class UserService : IUserService
    {
        private IUserRepo _userRepo;
        private IAuthorization _authorization;

        public UserService(IUserRepo userRepo, IAuthorization authorization)
        {
            _userRepo = userRepo;
            _authorization = authorization;
        }

        public async Task<List<UserDTO>?> GetUserAsync()
        {
            var users = await _userRepo.GetUserAsync();
            if (users == null) return null;
            return users.Select(u => new UserDTO
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                Role = u.UserRoles?.Select(ur => ur.Role.RoleName).FirstOrDefault() ?? "NA"
            }).ToList();   
        }

        public async Task<AuthResultDTO?> LoginAsync(LoginDTO loginDto)
        {
            var user = await _userRepo.LoginAsync(loginDto.Username, loginDto.Password);

            if (user == null)
            {
                return new AuthResultDTO { Succeeded = false, Message = "Invalid username or password." };
            }

            // TODO: Tạo JWT Token ở đây
            string token = _authorization.CreateAccessToken(user); // Đây là hàm sẽ tạo token

            return new AuthResultDTO { Succeeded = true, Message = "Login successful.", Token = token };

        }

        public async Task<AuthResultDTO> RegisterAsync(RegisterDTO register)
        {
            if (register.Password != register.ConfirmPassword)
            {
                return new AuthResultDTO { Succeeded = false, Message = "Passwords do not match." };
            }

            // 2. Chuyển đổi từ DTO sang Entity
            var userToCreate = new User
            {
                Username = register.Username,
                Email = register.Email
            };

            // 3. Gọi Repository để thực hiện thao tác với DB
            var createdUser = await _userRepo.ResgisterAsync(userToCreate, register.Password, register.ConfirmPassword);

            // Repository trả về null nếu username/email đã tồn tại
            if (createdUser == null)
            {
                return new AuthResultDTO { Succeeded = false, Message = "Username or email is already taken." };
            }

            return new AuthResultDTO { Succeeded = true, Message = "Registration successful." };
        }

        public Task<bool> UserExistsAsync(string username)
        {
            throw new NotImplementedException();
        }

    }
}
