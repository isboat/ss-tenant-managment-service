using Tenancy.Management.Models;
using Tenancy.Management.Mongo.Interfaces;
using Tenancy.Management.Services.Interfaces;

namespace Tenancy.Management.Services
{
    public class TenantService : ITenantService
    {
        private IRepository<TenantModel> _repository;

        public TenantService(IRepository<TenantModel> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<TenantModel>> GetTenantsAsync()
        {
            return await _repository.GetAsync();
        }

        public async Task<TenantModel> GetTenantAsync(string id)
        {
            return await _repository.GetAsync(id);
        }

        public async Task CreateAsync(TenantModel newModel)
        {
            await _repository.CreateAsync(newModel);
        }

        public async Task UpdateAsync(string id, TenantModel updatedModel)
        {
            await _repository.UpdateAsync(id, updatedModel);
        }

        public async Task RemoveAsync(string id)
        {
            await _repository.RemoveAsync(id);
        }
    }
}