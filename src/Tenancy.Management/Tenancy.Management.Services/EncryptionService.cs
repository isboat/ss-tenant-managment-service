using Tenancy.Management.Models;
using Tenancy.Management.Services.Interfaces;

namespace Tenancy.Management.Services
{
    public class EncryptionService : IEncryptionService
    {
        private readonly string _hashingSupport = "FA39DB22-672D-4B38-B96D-9905D6807447";

        public EncryptionService()
        {
        }

        public EncryptedResult? Encrypt(string input)
        {
            var salt = BCrypt.Net.BCrypt.GenerateSalt();

            // Generate a salt and hash the password
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(input + _hashingSupport, salt);

            // Store the hashed password in the database
            return new EncryptedResult { Hashed = hashedPassword, UsedSalt = salt };
        }

        public bool Verify(string input, string storedHash)
        {
            // Verify the entered password against the stored hash
            return BCrypt.Net.BCrypt.Verify(input + _hashingSupport, storedHash);
        }
    }
}
