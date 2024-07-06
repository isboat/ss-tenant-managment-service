using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Linq.Expressions;
using Tenancy.Management.Models;
using Tenancy.Management.Mongo.Interfaces;

namespace Tenancy.Management.Mongo
{
    public class RegistrationRepository : IRepository<RegisterModel>
    {
        private readonly IMongoCollection<RegisterModel> _collection;
        private readonly MongoClient _client;

        public RegistrationRepository(IOptions<MongoSettings> settings)
        {
            _client = new MongoClient(
            settings.Value.ConnectionString);

            var mongoDatabase = _client.GetDatabase(
                settings.Value.DatabaseName);

            _collection = mongoDatabase.GetCollection<RegisterModel>("Registrations");
        }

        public async Task<List<RegisterModel>> GetAsync() =>
            await _collection.Find(_ => true).ToListAsync();

        public async Task<RegisterModel?> GetAsync(string id) =>
            await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(RegisterModel newTenant) =>
            await _collection.InsertOneAsync(newTenant);

        public async Task UpdateAsync(string id, RegisterModel updatedBook) =>
            await _collection.ReplaceOneAsync(x => x.Id == id, updatedBook);

        public async Task RemoveAsync(string id) =>
            await _collection.DeleteOneAsync(x => x.Id == id);

        public void CreateDB(string dbName)
        {
            var db = _client.GetDatabase(dbName);
            db.CreateCollection("TenantInfo");
        }

        public Task<IEnumerable<RegisterModel>> GetByFilterAsync(Expression<Func<RegisterModel, bool>> filter)
        {
            throw new NotImplementedException();
        }
    }
}