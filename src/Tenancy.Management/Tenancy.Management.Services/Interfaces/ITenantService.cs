using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenancy.Management.Models;

namespace Tenancy.Management.Services.Interfaces
{
    public interface ITenantService
    {
        Task<IEnumerable<TenantModel>> GetTenantsAsync();

        Task<TenantModel> GetTenantAsync(string id);

        public Task CreateAsync(TenantModel newModel);

        public Task UpdateAsync(string id, TenantModel updatedModel);

        public Task RemoveAsync(string id);
    }
}
