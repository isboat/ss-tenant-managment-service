using Microsoft.Extensions.Options;
using Tenancy.Management.Models;
using Tenancy.Management.Mongo.Interfaces;
using Tenancy.Management.Services.Interfaces;

namespace Tenancy.Management.Services
{
    public class UserService : IUserService
    {
        private IUserRepository _repository;

        private readonly string _hashingSupport = "FA39DB22-672D-4B38-B96D-9905D6807447";

        public UserService(IUserRepository repository)
        {
            _repository = repository;
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

        public async Task CreateAsync(UserModel newModel)
        {
            EnsureIdNotNull(newModel);
            newModel.Password = Encrypt("Temporary!")?.Hashed;

            await _repository.CreateAsync(newModel);
        }

        public async Task UpdateAsync(string id, UserModel updatedModel)
        {
            if (updatedModel == null) return;

            EnsureIdNotNull(updatedModel);
            updatedModel.Password = Encrypt(updatedModel.Password!)?.Hashed;
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

        private EncryptedResult? Encrypt(string input)
        {
            var salt = BCrypt.Net.BCrypt.GenerateSalt();

            // Generate a salt and hash the password
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(input + _hashingSupport, salt);

            // Store the hashed password in the database
            return new EncryptedResult { Hashed = hashedPassword, UsedSalt = salt };
        }

        private bool Verify(string input, string storedHash)
        {
            // Verify the entered password against the stored hash
            return BCrypt.Net.BCrypt.Verify(input + _hashingSupport, storedHash);
        }
    }
}