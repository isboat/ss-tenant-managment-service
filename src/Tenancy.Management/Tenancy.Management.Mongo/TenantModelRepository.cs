using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Linq.Expressions;
using Tenancy.Management.Models;
using Tenancy.Management.Mongo.Interfaces;

namespace Tenancy.Management.Mongo
{
    public class TenantModelRepository<T> : ITenantRepository<T> where T : IModelItem, new()
    {
        private readonly MongoClient _client;
        private readonly string _collectionName;

        public TenantModelRepository(IOptions<MongoSettings> settings, string collectionName)
        {
            _client = new MongoClient(settings.Value.ConnectionString);
            _collectionName = collectionName;
        }

        private IMongoCollection<T> GetCollection(string tenantId)
        {
            var mongoDatabase = _client.GetDatabase(tenantId);

            var collection = mongoDatabase.GetCollection<T>(_collectionName);
            return collection;
        }

        public async Task<List<T>> GetAsync(string tenantId)
        {
            var collection = GetCollection(tenantId);
            return await collection.Find(_ => true).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetByFilterAsync(string tenantId, FilterDefinition<T> filter)
        {
            var collection = GetCollection(tenantId);
            return await collection.Find(filter)
            .ToListAsync();
        }

        public async Task<T?> GetAsync(string tenantId, string id)
        {
            var collection = GetCollection(tenantId);
            return await collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(string tenantId, T newModel)
        {
            var collection = GetCollection(tenantId);
            await collection.InsertOneAsync(newModel);
        }

        public async Task UpdateAsync(string tenantId, string id, T updatedModel)
        {
            var collection = GetCollection(tenantId);
            await collection.ReplaceOneAsync(x => x.Id == id, updatedModel);
        }

        public async Task RemoveAsync(string tenantId, string id)
        {
            var collection = GetCollection(tenantId);
            await collection.DeleteOneAsync(x => x.Id == id);
        }
    }
}