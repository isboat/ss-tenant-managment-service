using Tenancy.Management.Models;
using Tenancy.Management.Mongo.Interfaces;
using Tenancy.Management.Services;

namespace Tenancy.Management.Services.Tests;

public class TenantServiceTests
{
    [Fact]
    public async Task CreateAsync_NormalizesTenantId_ForMongoDatabaseName()
    {
        var repository = new InMemoryTenantRepository();
        var service = new TenantService(repository);
        var tenant = new TenantModel { Name = " ACME / North #1 " };

        await service.CreateAsync(tenant);

        Assert.Equal("acme_north_1_tenant", tenant.Id);
        Assert.Equal("acme_north_1_tenant", repository.CreatedDatabaseName);
    }

    private sealed class InMemoryTenantRepository : ITenantDBRepository<TenantModel>
    {
        public string? CreatedDatabaseName { get; private set; }
        public Task<List<TenantModel>> GetAsync() => Task.FromResult(new List<TenantModel>());
        public IEnumerable<TenantModel> GetByFilter(Func<TenantModel, bool> filter) => Enumerable.Empty<TenantModel>();
        public Task<TenantModel?> GetAsync(string id) => Task.FromResult<TenantModel?>(null);
        public Task CreateAsync(TenantModel newModel) => Task.CompletedTask;
        public Task UpdateAsync(string id, TenantModel updatedModel) => Task.CompletedTask;
        public Task RemoveAsync(string id) => Task.CompletedTask;
        public void CreateDB(string id) => CreatedDatabaseName = id;
    }
}
