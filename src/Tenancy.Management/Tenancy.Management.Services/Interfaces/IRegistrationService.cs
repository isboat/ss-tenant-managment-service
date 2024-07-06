using Tenancy.Management.Models;

namespace Tenancy.Management.Services.Interfaces
{
    public interface IRegistrationService
    {
        Task<IEnumerable<RegisterModel>> GetTenantsAsync();

        Task<RegisterModel> GetTenantAsync(string id);

        public Task RemoveAsync(string id);
    }
}
