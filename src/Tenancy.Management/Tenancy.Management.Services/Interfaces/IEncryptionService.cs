using Tenancy.Management.Models;

namespace Tenancy.Management.Services.Interfaces
{
    public interface IEncryptionService
    {
        EncryptedResult? Encrypt(string input);
        bool Verify(string input, string storedHash);
    }
}
