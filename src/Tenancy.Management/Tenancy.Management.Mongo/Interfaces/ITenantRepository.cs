using MongoDB.Driver;
using System.Linq.Expressions;

namespace Tenancy.Management.Mongo.Interfaces
{
    public interface ITenantRepository<T>
    {
        public Task<List<T>> GetAsync(string tenantId);

        public Task<IEnumerable<T>> GetByFilterAsync(string tenantId, FilterDefinition<T> filter);

        public Task<T?> GetAsync(string tenantId, string id);

        public Task CreateAsync(string tenantId, T newModel);

        public Task UpdateAsync(string tenantId, string id, T updatedModel);

        public Task RemoveAsync(string tenantId, string id);
    }
}
