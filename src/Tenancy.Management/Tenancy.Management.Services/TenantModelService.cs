using MongoDB.Driver;
using System.Linq.Expressions;
using Tenancy.Management.Models;
using Tenancy.Management.Mongo.Interfaces;
using Tenancy.Management.Services.Interfaces;

namespace Tenancy.Management.Services
{
    public class TenantModelService<T> : ITenantModelService<T> where T : IModelItem, new()
    {
        private readonly ITenantRepository<T> _repository;

        public TenantModelService(ITenantRepository<T> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<T>> GetAllAsync(string tenantId)
        {
            return await _repository.GetAsync(tenantId);
        }

        public async Task<IEnumerable<T>> GetByFilterAsync(string tenantId, FilterDefinition<T> filter)
        {
            return await _repository.GetByFilterAsync(tenantId, filter);
        }

        public async Task<T?> GetAsync(string tenantId, string id)
        {
            return await _repository.GetAsync(tenantId, id);
        }

        public async Task UpdateAsync(string tenantId, string id, T updatedModel)
        {
            await _repository.UpdateAsync(tenantId, id, updatedModel);
        }

        public async Task RemoveAsync(string tenantId, string id)
        {
            await _repository.RemoveAsync(tenantId, id);
        }
    }
}