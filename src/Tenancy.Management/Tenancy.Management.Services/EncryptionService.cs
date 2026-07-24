using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using Tenancy.Management.Models;
using Tenancy.Management.Services.Interfaces;

namespace Tenancy.Management.Services
{
    public class EncryptionService : IEncryptionService
    {
        private readonly string _passwordPepper;

        public EncryptionService(IOptions<AuthSettings> settings)
        {
            _passwordPepper = settings.Value.PasswordPepper ?? throw new InvalidOperationException("AuthSettings:PasswordPepper must be configured.");
        }

        public EncryptedResult? Encrypt(string input)
        {
            var salt = BCrypt.Net.BCrypt.GenerateSalt();

            // Generate a salt and hash the password
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(input + _passwordPepper, salt);

            // Store the hashed password in the database
            return new EncryptedResult { Hashed = hashedPassword, UsedSalt = salt };
        }

        public bool Verify(string input, string storedHash)
        {
            // Verify the entered password against the stored hash
            return BCrypt.Net.BCrypt.Verify(input + _passwordPepper, storedHash);
        }

        public string GenerateToken(int byteLength = 32)
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(byteLength));
        }

        public string HashToken(string token)
        {
            var bytes = SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(token + _passwordPepper));
            return Convert.ToHexString(bytes);
        }
    }
}
