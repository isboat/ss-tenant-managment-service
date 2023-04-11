using Tenancy.Management.Models;
using Tenancy.Management.Mongo.Interfaces;
using Tenancy.Management.Services.Interfaces;

namespace Tenancy.Management.Services
{
    public class UserService : IUserService
    {
        private IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<UserModel>> GetUsersAsync(string tenantId)
        {
            return await _repository.GetUsersAsync(tenantId);
        }

        public async Task<UserModel> GetAsync(string id)
        {
            return await _repository.GetAsync(id);
        }

        public async Task CreateAsync(UserModel newModel)
        {
            EnsureIdNotNull(newModel);

            await _repository.CreateAsync(newModel);
        }

        public async Task UpdateAsync(string id, UserModel updatedModel)
        {
            EnsureIdNotNull(updatedModel);
            await _repository.UpdateAsync(id, updatedModel);
        }

        public async Task RemoveAsync(string id)
        {
            await _repository.RemoveAsync(id);
        }


        private static void EnsureIdNotNull(UserModel newModel)
        {
            if (string.IsNullOrEmpty(newModel?.Id))
            {
                throw new ArgumentNullException(nameof(newModel.Id));
            }

            if (string.IsNullOrEmpty(newModel?.TenantId))
            {
                throw new ArgumentNullException(nameof(newModel.TenantId));
            }
        }
    }
}