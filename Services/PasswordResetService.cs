using efcoreApi.Data;
using efcoreApi.Models;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;
using System.Security.Cryptography;

namespace efcoreApi.Services
{
    public class PasswordResetService
    {
        private readonly efContext _context;
        private readonly IConfiguration configuration;

        public PasswordResetService(efContext context, IConfiguration _configuration)
        {
            _context = context;
            configuration=_configuration;
        }

        public async Task<string> GeneratePasswordResetTokenAsync(string userId)
        {
            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var token = Convert.ToBase64String(tokenBytes);

            var resetToken = new PasswordResetToken
            {
                RegisterId = userId,
                Token = token,
                ExpirationDate = DateTime.UtcNow.AddMinutes(30),
                IsUsed = false
            };

            _context.passwordResetTokens.Add(resetToken);
            await _context.SaveChangesAsync();

            return token;
        }

        public async Task<bool> ResetPasswordAsync(string email, string token, string newPassword)
        {
            var user = await _context.Register.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return false;

            var resetToken = await _context.passwordResetTokens
                .FirstOrDefaultAsync(t => t.RegisterId == user.Id && t.Token == token && !t.IsUsed);

            if (resetToken == null || resetToken.ExpirationDate < DateTime.UtcNow)
                return false;

            // Mark token as used
            resetToken.IsUsed = true;

            // Update user password
            //user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            user.Password = EncDec.EncryptString(configuration["EnkDec:key"], newPassword);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
