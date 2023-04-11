using Tenancy.Management.Models;

namespace Tenancy.Management.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserModel>> GetUsersAsync(string tenantId);

        Task<UserModel> GetAsync(string id);

        public Task CreateAsync(UserModel newModel);

        public Task UpdateAsync(string id, UserModel updatedModel);

        public Task RemoveAsync(string id);
    }
}
