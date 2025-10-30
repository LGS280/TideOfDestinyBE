using System.Threading.Tasks;
using TideOfDestiniy.DAL.Entities;

namespace TideOfDestiniy.DAL.Interfaces
{
    public interface IPasswordResetTokenRepo
    {
        Task<PasswordResetToken> CreateTokenAsync(string email, string token, DateTime expiresAt);
        Task<PasswordResetToken?> GetValidTokenAsync(string email, string token);
        Task<bool> MarkTokenAsUsedAsync(Guid tokenId);
        Task<bool> CleanupExpiredTokensAsync();
    }
}
