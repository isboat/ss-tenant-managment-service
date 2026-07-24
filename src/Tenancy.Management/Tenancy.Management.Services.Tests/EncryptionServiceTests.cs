using Microsoft.Extensions.Options;
using Tenancy.Management.Models;
using Tenancy.Management.Services;
using Xunit;

namespace Tenancy.Management.Services.Tests;

public class EncryptionServiceTests
{
    [Fact]
    public void Verify_ReturnsTrue_ForMatchingPassword()
    {
        var service = CreateService();

        var encrypted = service.Encrypt("correct horse battery staple");

        Assert.NotNull(encrypted);
        Assert.True(service.Verify("correct horse battery staple", encrypted.Hashed));
    }

    [Fact]
    public void HashToken_ReturnsStableHash_WithoutReturningRawToken()
    {
        var service = CreateService();
        var token = service.GenerateToken();

        var firstHash = service.HashToken(token);
        var secondHash = service.HashToken(token);

        Assert.Equal(firstHash, secondHash);
        Assert.NotEqual(token, firstHash);
    }

    private static EncryptionService CreateService()
    {
        return new EncryptionService(Options.Create(new AuthSettings { PasswordPepper = "test-pepper" }));
    }
}
