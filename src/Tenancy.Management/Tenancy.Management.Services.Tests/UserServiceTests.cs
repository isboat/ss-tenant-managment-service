using Microsoft.Extensions.Options;
using Tenancy.Management.Models;
using Tenancy.Management.Mongo.Interfaces;
using Tenancy.Management.Services;

namespace Tenancy.Management.Services.Tests;

public class UserServiceTests
{
    [Fact]
    public async Task CreateAsync_ReturnsRawInviteToken_AndStoresOnlyHash()
    {
        var repository = new InMemoryUserRepository();
        var service = CreateService(repository);
        var user = new UserModel { Id = "user-1", TenantId = "tenant-a", Email = "user@example.com" };

        var inviteToken = await service.CreateAsync(user);

        Assert.False(string.IsNullOrWhiteSpace(inviteToken));
        Assert.Null(repository.CreatedUser?.Password);
        Assert.NotEqual(inviteToken, repository.CreatedUser?.InviteTokenHash);
        Assert.False(string.IsNullOrWhiteSpace(repository.CreatedUser?.InviteTokenHash));
    }

    [Fact]
    public async Task UpdateAsync_Throws_WhenUserDoesNotBelongToTenant()
    {
        var repository = new InMemoryUserRepository
        {
            ExistingUser = new UserModel { Id = "user-1", TenantId = "tenant-b", Password = "existing-hash" }
        };
        var service = CreateService(repository);
        var update = new UserModel { Id = "user-1", TenantId = "tenant-a", Email = "user@example.com" };

        await Assert.ThrowsAsync<KeyNotFoundException>(() => service.UpdateAsync("user-1", update));
    }

    private static UserService CreateService(IUserRepository repository)
    {
        var authSettings = Options.Create(new AuthSettings
        {
            PasswordPepper = "test-pepper",
            InviteTokenExpiryHours = 24,
        });

        return new UserService(repository, new EncryptionService(authSettings), authSettings);
    }

    private sealed class InMemoryUserRepository : IUserRepository
    {
        public UserModel? CreatedUser { get; private set; }
        public UserModel? ExistingUser { get; init; }

        public Task<List<UserModel>> GetUsersAsync(string tenantId) => Task.FromResult(new List<UserModel>());
        public Task<UserModel?> GetByEmailAsync(string email) => Task.FromResult<UserModel?>(null);
        public Task<UserModel?> GetByTenantAsync(string tenantId, string id) =>
            Task.FromResult(ExistingUser?.TenantId == tenantId && ExistingUser.Id == id ? ExistingUser : null);
        public Task RemoveAsync(string tenantId, string id) => Task.CompletedTask;
        public Task<List<UserModel>> GetAsync() => Task.FromResult(new List<UserModel>());
        public IEnumerable<UserModel> GetByFilter(Func<UserModel, bool> filter) => Enumerable.Empty<UserModel>();
        public Task<UserModel?> GetAsync(string id) => Task.FromResult(ExistingUser?.Id == id ? ExistingUser : null);
        public Task CreateAsync(UserModel newModel)
        {
            CreatedUser = newModel;
            return Task.CompletedTask;
        }
        public Task UpdateAsync(string id, UserModel updatedModel) => Task.CompletedTask;
        public Task RemoveAsync(string id) => Task.CompletedTask;
    }
}
