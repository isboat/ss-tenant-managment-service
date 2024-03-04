using MongoDB.Driver;
using System.Linq.Expressions;

namespace Tenancy.Management.Services.Interfaces
{
    public interface ITenantModelService<T>
    {
        Task<IEnumerable<T>> GetAllAsync(string tenantId);

        Task<IEnumerable<T>> GetByFilterAsync(string tenantId, FilterDefinition<T> filter);

        Task<T?> GetAsync(string tenantId, string id);

        public Task UpdateAsync(string tenantId, string id, T updatedModel);

        public Task RemoveAsync(string tenantId, string id);
    }
}
