using Microsoft.Extensions.Options;
using Tenancy.Management.Models;
using Tenancy.Management.Mongo.Interfaces;
using Tenancy.Management.Services.Interfaces;

namespace Tenancy.Management.Services
{
    public class UserService : IUserService
    {
        private IUserRepository _repository;
        private readonly IEncryptionService _encryptionService;
        private readonly AuthSettings _authSettings;

        public UserService(IUserRepository repository, IEncryptionService encryptionService, IOptions<AuthSettings> authSettings)
        {
            _repository = repository;
            _encryptionService = encryptionService;
            _authSettings = authSettings.Value;
        }

        public async Task<IEnumerable<UserModel>> GetUsersAsync(string tenantId)
        {
            var models = await _repository.GetUsersAsync(tenantId);
            models.ForEach(x => x.Password = null);

            return models;
        }

        public async Task<UserModel> GetAsync(string id)
        {
            var model = await _repository.GetAsync(id);
            if(model != null) model.Password = null;

            return model!;
        }

        public async Task<UserModel?> GetAsync(string tenantId, string id)
        {
            var model = await _repository.GetByTenantAsync(tenantId, id);
            if (model != null) model.Password = null;

            return model;
        }

        public async Task<UserModel> GetByEmailAsync(string email)
        {
            var model = await _repository.GetByEmailAsync(email);
            if (model != null) model.Password = null;

            return model!;
        }

        public async Task<string> CreateAsync(UserModel newModel)
        {
            EnsureIdNotNull(newModel);
            var inviteToken = _encryptionService.GenerateToken();
            newModel.Password = null;
            newModel.InviteTokenHash = _encryptionService.HashToken(inviteToken);
            newModel.InviteTokenExpiresOn = DateTime.UtcNow.AddHours(_authSettings.InviteTokenExpiryHours);
            newModel.InviteTokenConsumedOn = null;

            await _repository.CreateAsync(newModel);
            return inviteToken;
        }

        public async Task UpdateAsync(string id, UserModel updatedModel)
        {
            if (updatedModel == null) return;

            EnsureIdNotNull(updatedModel);
            var existingModel = await _repository.GetByTenantAsync(updatedModel.TenantId!, id);
            if (existingModel == null)
            {
                throw new KeyNotFoundException($"User '{id}' was not found for tenant '{updatedModel.TenantId}'.");
            }

            updatedModel.Password = string.IsNullOrWhiteSpace(updatedModel.Password)
                ? existingModel?.Password
                : _encryptionService.Encrypt(updatedModel.Password)?.Hashed;
            await _repository.UpdateAsync(id, updatedModel);
        }

        public async Task RemoveAsync(string id)
        {
            await _repository.RemoveAsync(id);
        }

        public async Task RemoveAsync(string tenantId, string id)
        {
            await _repository.RemoveAsync(tenantId, id);
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
