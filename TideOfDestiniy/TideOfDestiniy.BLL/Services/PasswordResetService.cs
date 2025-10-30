using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TideOfDestiniy.BLL.DTOs.Requests;
using TideOfDestiniy.BLL.DTOs.Responses;
using TideOfDestiniy.BLL.Interfaces;
using TideOfDestiniy.DAL.Interfaces;
using BCrypt.Net;

namespace TideOfDestiniy.BLL.Services
{
    public class PasswordResetService : IPasswordResetService
    {
        private readonly IUserRepo _userRepo;
        private readonly IPasswordResetTokenRepo _tokenRepo;

        public PasswordResetService(IUserRepo userRepo, IPasswordResetTokenRepo tokenRepo)
        {
            _userRepo = userRepo;
            _tokenRepo = tokenRepo;
        }

        public async Task<PasswordResetResultDTO> ForgotPasswordAsync(ForgotPasswordDTO forgotPasswordDto)
        {
            try
            {
                // Check if user exists
                var user = await _userRepo.GetUserByEmailAsync(forgotPasswordDto.Email);
                if (user == null)
                {
                    // For security, don't reveal if email exists or not
                    return new PasswordResetResultDTO
                    {
                        Succeeded = true,
                        Message = "If the email exists in our system, a password reset link has been sent."
                    };
                }

                // Generate reset token
                var token = GenerateResetToken();
                var expiresAt = DateTime.UtcNow.AddHours(1); // Token expires in 1 hour

                // Store token in database
                await _tokenRepo.CreateTokenAsync(forgotPasswordDto.Email, token, expiresAt);

                // TODO: Send email with reset link
                // For now, we'll return the token in the response for testing
                return new PasswordResetResultDTO
                {
                    Succeeded = true,
                    Message = $"Password reset token generated for {forgotPasswordDto.Email}",
                    Token = token // Return the token for testing purposes
                };
            }
            catch (Exception ex)
            {
                return new PasswordResetResultDTO
                {
                    Succeeded = false,
                    Message = "An error occurred while processing your request."
                };
            }
        }

        public async Task<PasswordResetResultDTO> ResetPasswordAsync(ResetPasswordRequestDTO resetPasswordDto)
        {
            try
            {
                // Validate token
                var tokenEntity = await _tokenRepo.GetValidTokenAsync(resetPasswordDto.Email, resetPasswordDto.Token);
                if (tokenEntity == null)
                {
                    return new PasswordResetResultDTO
                    {
                        Succeeded = false,
                        Message = "Invalid or expired reset token."
                    };
                }

                // Get user
                var user = await _userRepo.GetUserByEmailAsync(resetPasswordDto.Email);
                if (user == null)
                {
                    return new PasswordResetResultDTO
                    {
                        Succeeded = false,
                        Message = "User not found."
                    };
                }

                // Hash new password
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(resetPasswordDto.NewPassword);
                
                // Update user password
                user.PasswordHash = hashedPassword;
                var updateResult = await _userRepo.UpdateUserAsync(user);

                if (!updateResult)
                {
                    return new PasswordResetResultDTO
                    {
                        Succeeded = false,
                        Message = "Failed to update password."
                    };
                }

                // Mark token as used
                await _tokenRepo.MarkTokenAsUsedAsync(tokenEntity.Id);

                return new PasswordResetResultDTO
                {
                    Succeeded = true,
                    Message = "Password has been reset successfully."
                };
            }
            catch (Exception ex)
            {
                return new PasswordResetResultDTO
                {
                    Succeeded = false,
                    Message = "An error occurred while resetting your password."
                };
            }
        }

        private string GenerateResetToken()
        {
            using var rng = RandomNumberGenerator.Create();
            var bytes = new byte[32];
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes).Replace("+", "-").Replace("/", "_").Replace("=", "");
        }
    }
}
