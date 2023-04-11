using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Tenancy.Management.Models;
using Tenancy.Management.Mongo.Interfaces;

namespace Tenancy.Management.Mongo
{
    public class UserRepository: IUserRepository
    {
        private readonly IMongoCollection<UserModel> _collection;
        private readonly MongoClient _client;

        public UserRepository(IOptions<MongoSettings> settings)
        {
            _client = new MongoClient(
            settings.Value.ConnectionString);

            var mongoDatabase = _client.GetDatabase("ServiceUsersAccount");

            _collection = mongoDatabase.GetCollection<UserModel>("Users");
        }

        public async Task<List<UserModel>> GetUsersAsync(string tenantId) =>
            await _collection.Find(x => x.TenantId == tenantId).ToListAsync();

        public async Task<List<UserModel>> GetAsync() =>
            await _collection.Find(_ => true).ToListAsync();

        public async Task<UserModel?> GetAsync(string id) =>
            await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(UserModel newTenant) =>
            await _collection.InsertOneAsync(newTenant);

        public async Task UpdateAsync(string id, UserModel updatedBook) =>
            await _collection.ReplaceOneAsync(x => x.Id == id, updatedBook);

        public async Task RemoveAsync(string id) =>
            await _collection.DeleteOneAsync(x => x.Id == id);
    }
}