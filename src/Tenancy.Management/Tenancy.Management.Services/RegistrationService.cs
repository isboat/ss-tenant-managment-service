using Tenancy.Management.Models;
using Tenancy.Management.Mongo.Interfaces;
using Tenancy.Management.Services.Interfaces;

namespace Tenancy.Management.Services
{
    public class RegistrationService : IRegistrationService
    {
        private IRepository<RegisterModel> _repository;

        public RegistrationService(IRepository<RegisterModel> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<RegisterModel>> GetTenantsAsync()
        {
            return await _repository.GetAsync();
        }

        public async Task<RegisterModel> GetTenantAsync(string id)
        {
            return await _repository.GetAsync(id);
        }

        public async Task RemoveAsync(string id)
        {
            await _repository.RemoveAsync(id);
        }
    }
}