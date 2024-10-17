using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Linq.Expressions;
using Tenancy.Management.Models;
using Tenancy.Management.Mongo.Interfaces;

namespace Tenancy.Management.Mongo
{
    public class TenantAdminRepository: ITenantDBRepository<TenantModel>
    {
        private readonly IMongoCollection<TenantModel> _collection;
        private readonly MongoClient _client;

        public TenantAdminRepository(IOptions<MongoSettings> settings)
        {
            _client = new MongoClient(
            settings.Value.ConnectionString);

            var mongoDatabase = _client.GetDatabase(
                settings.Value.DatabaseName);

            _collection = mongoDatabase.GetCollection<TenantModel>(
                settings.Value.CollectionName);
        }

        public async Task<List<TenantModel>> GetAsync() =>
            await _collection.Find(_ => true).ToListAsync();

        public async Task<TenantModel?> GetAsync(string id) =>
            await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(TenantModel newTenant) =>
            await _collection.InsertOneAsync(newTenant);

        public async Task UpdateAsync(string id, TenantModel updatedBook) =>
            await _collection.ReplaceOneAsync(x => x.Id == id, updatedBook);

        public async Task RemoveAsync(string id) =>
            await _collection.DeleteOneAsync(x => x.Id == id);

        public void CreateDB(string dbName)
        {
            var db = _client.GetDatabase(dbName);
            db.CreateCollection("TenantInfo");
        }

        public IEnumerable<TenantModel> GetByFilter(Func<TenantModel, bool> filter)
        {
            return _collection.AsQueryable().Where(filter);
        }
    }
}