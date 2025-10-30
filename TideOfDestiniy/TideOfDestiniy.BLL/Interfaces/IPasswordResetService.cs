using System.Threading.Tasks;
using TideOfDestiniy.BLL.DTOs.Requests;
using TideOfDestiniy.BLL.DTOs.Responses;

namespace TideOfDestiniy.BLL.Interfaces
{
    public interface IPasswordResetService
    {
        Task<PasswordResetResultDTO> ForgotPasswordAsync(ForgotPasswordDTO forgotPasswordDto);
        Task<PasswordResetResultDTO> ResetPasswordAsync(ResetPasswordRequestDTO resetPasswordDto);
    }
}
