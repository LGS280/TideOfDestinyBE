using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TideOfDestiniy.DAL.Context;
using TideOfDestiniy.DAL.Entities;
using TideOfDestiniy.DAL.Interfaces;

namespace TideOfDestiniy.DAL.Repositories
{
    public class PasswordResetTokenRepo : IPasswordResetTokenRepo
    {
        private readonly TideOfDestinyDbContext _context;

        public PasswordResetTokenRepo(TideOfDestinyDbContext context)
        {
            _context = context;
        }

        public async Task<PasswordResetToken> CreateTokenAsync(string email, string token, DateTime expiresAt)
        {
            // Invalidate any existing tokens for this email
            var existingTokens = await _context.PasswordResetTokens
                .Where(t => t.Email.ToLower() == email.ToLower() && !t.IsUsed)
                .ToListAsync();

            foreach (var existingToken in existingTokens)
            {
                existingToken.IsUsed = true;
            }

            var newToken = new PasswordResetToken
            {
                Id = Guid.NewGuid(),
                Email = email.ToLower(),
                Token = token,
                ExpiresAt = expiresAt,
                CreatedAt = DateTime.UtcNow,
                IsUsed = false
            };

            await _context.PasswordResetTokens.AddAsync(newToken);
            await _context.SaveChangesAsync();

            return newToken;
        }

        public async Task<PasswordResetToken?> GetValidTokenAsync(string email, string token)
        {
            return await _context.PasswordResetTokens
                .FirstOrDefaultAsync(t => 
                    t.Email.ToLower() == email.ToLower() && 
                    t.Token == token && 
                    !t.IsUsed && 
                    t.ExpiresAt > DateTime.UtcNow);
        }

        public async Task<bool> MarkTokenAsUsedAsync(Guid tokenId)
        {
            var token = await _context.PasswordResetTokens.FindAsync(tokenId);
            if (token == null)
            {
                return false;
            }

            token.IsUsed = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CleanupExpiredTokensAsync()
        {
            var expiredTokens = await _context.PasswordResetTokens
                .Where(t => t.ExpiresAt <= DateTime.UtcNow)
                .ToListAsync();

            if (expiredTokens.Any())
            {
                _context.PasswordResetTokens.RemoveRange(expiredTokens);
                await _context.SaveChangesAsync();
            }

            return true;
        }
    }
}
