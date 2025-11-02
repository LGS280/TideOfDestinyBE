using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TideOfDestiniy.BLL.Interfaces;
using TideOfDestiniy.DAL.Entities;
using TideOfDestiniy.DAL.Interfaces;
using TideOfDestiniy.DAL.Repositories;
using Google.Apis.Auth;
using TideOfDestiniy.BLL.DTOs.Responses; // Thêm using


namespace TideOfDestiniy.BLL.Services
{
    public class Authorization : IAuthorization, IAuthService
    {
        private IConfiguration _configuration;
        private IUserRepo _userRepo;

        public Authorization(IConfiguration configuration, IUserRepo userRepo)
        {
            _configuration = configuration;
            _userRepo = userRepo;
        }
       public string CreateAccessToken(User user)
{
    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
    var creds = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256Signature);

    var now = DateTime.UtcNow;
    var expires = now.AddHours(Convert.ToDouble(_configuration["JwtSettings:AccessTokenExpirationHours"]));

    var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Name, user.Username),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
    };

    if (user.UserRoles != null)
    {
        foreach (var userRole in user.UserRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, userRole.Role.RoleName));
        }
    }

    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity(claims),
        IssuedAt = now,
        NotBefore = now,
        Expires = expires,
        SigningCredentials = creds,
        Issuer = _configuration["JwtSettings:Issuer"],
        Audience = _configuration["JwtSettings:Audience"]
    };

    var tokenHandler = new JwtSecurityTokenHandler();
    var token = tokenHandler.CreateToken(tokenDescriptor);

    return tokenHandler.WriteToken(token);
}


        public string CreateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public async Task<AuthResultDTO> LoginWithGoogleAsync(GoogleLoginDTO googleLoginDto)
        {
            var googleClientId = _configuration["GoogleAuthSettings:ClientId"];
            if (string.IsNullOrEmpty(googleClientId))
            {
                return new AuthResultDTO { Succeeded = false, Message = "Google Client ID is not configured." };
            }

            try
            {
                // Bước 1: Xác thực idToken với Google
                var validationSettings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { googleClientId }
                };
                var payload = await GoogleJsonWebSignature.ValidateAsync(googleLoginDto.idToken, validationSettings);

                // Bước 2: Tìm hoặc tạo người dùng trong hệ thống của bạn
                var user = await _userRepo.GetUserByEmailAsync(payload.Email); // Cần tạo hàm này trong Repo

                if (user == null)
                {
                    // Nếu người dùng chưa tồn tại, tạo mới
                    user = new User
                    {
                        Id = Guid.NewGuid(),
                        Email = payload.Email,
                        Username = payload.Name, // Hoặc tạo username duy nhất từ email
                        EmailConfirmed = payload.EmailVerified,
                        PasswordHash = null // <<-- Đặt là null một cách tường minh

                        // Không có PasswordHash vì họ đăng nhập qua Google
                    };
                    user = await _userRepo.CreateUserAsync(user); // Cần tạo hàm này trong Repo
                }

                // Bước 3: Tạo token của hệ thống bạn và trả về
                var accessToken = CreateAccessToken(user);
                var refreshToken = CreateRefreshToken();

                //user.RefreshToken = refreshToken;
                //user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
                await _userRepo.UpdateUserAsync(user);

                return new AuthResultDTO
                {
                    Succeeded = true,
                    Message = "Google login successful.",
                    Token = accessToken,
                };
            }
            catch (InvalidJwtException ex)
            {
                // Token không hợp lệ
                return new AuthResultDTO { Succeeded = false, Message = "Invalid Google token." };
            }
            catch (Exception ex)
            {
                // Lỗi không xác định
                return new AuthResultDTO { Succeeded = false, Message = $"An error occurred: {ex.Message}" };
            }
        }
    }
}
