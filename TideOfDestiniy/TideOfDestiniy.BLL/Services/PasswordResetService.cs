using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
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
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public PasswordResetService(
            IUserRepo userRepo, 
            IPasswordResetTokenRepo tokenRepo,
            IEmailService emailService,
            IConfiguration configuration)
        {
            _userRepo = userRepo;
            _tokenRepo = tokenRepo;
            _emailService = emailService;
            _configuration = configuration;
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

                // Get client domain from configuration
                var clientDomain = _configuration["ClientDomain"] ?? "https://tide-of-destiny-client.vercel.app";
                
                // Build reset password link
                var resetLink = $"{clientDomain}/reset-password?email={Uri.EscapeDataString(forgotPasswordDto.Email)}&token={Uri.EscapeDataString(token)}";

                // Send email with reset link
                try
                {
                    await _emailService.SendPasswordResetEmailAsync(
                        forgotPasswordDto.Email, 
                        resetLink, 
                        user.Username ?? user.Email);
                }
                catch (Exception emailEx)
                {
                    // Log email error with full details for debugging
                    Console.WriteLine($"Failed to send password reset email: {emailEx.Message}");
                    Console.WriteLine($"Stack trace: {emailEx.StackTrace}");
                    if (emailEx.InnerException != null)
                    {
                        Console.WriteLine($"Inner exception: {emailEx.InnerException.Message}");
                    }
                    
                    // In development, return error with details for debugging
                    // In production, you might want to log and still return success
                    return new PasswordResetResultDTO
                    {
                        Succeeded = false,
                        Message = $"Failed to send email: {emailEx.Message}"
                    };
                }

                // Always return success message for security (don't reveal if email exists)
                return new PasswordResetResultDTO
                {
                    Succeeded = true,
                    Message = "If the email exists in our system, a password reset link has been sent."
                };
            }
            catch (Exception ex)
            {
                // Log the full error for debugging
                Console.WriteLine($"Password reset error: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                
                // In development, return detailed error for debugging
                // TODO: Remove detailed error in production
                return new PasswordResetResultDTO
                {
                    Succeeded = false,
                    Message = $"An error occurred while processing your request: {ex.Message}"
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
