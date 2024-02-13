using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Linq.Expressions;
using Tenancy.Management.Models;
using Tenancy.Management.Mongo.Interfaces;

namespace Tenancy.Management.Mongo
{
    public class DeviceRepository : IRepository<DeviceAuthModel>
    {
        private readonly IMongoCollection<DeviceAuthModel> _collection;
        private readonly MongoClient _client;

        public DeviceRepository(IOptions<MongoSettings> settings)
        {
            _client = new MongoClient(
            settings.Value.ConnectionString);

            var mongoDatabase = _client.GetDatabase("device-app-db");

            _collection = mongoDatabase.GetCollection<DeviceAuthModel>("DeviceCodeRegistration");
        }

        public async Task<List<DeviceAuthModel>> GetAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<IEnumerable<DeviceAuthModel>> GetByFilterAsync(Expression<Func<DeviceAuthModel, bool>> filter)
        {
            return await _collection.Find(x => 
                x.TenantId == null                
                && x.ApprovedDatetime == null
                && x.RegisteredDatetime < DateTime.UtcNow.AddDays(-1))
            .ToListAsync();
        }

        public async Task<DeviceAuthModel?> GetAsync(string id)
        {
            return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(DeviceAuthModel newModel)
        {
            await _collection.InsertOneAsync(newModel);
        }

        public async Task UpdateAsync(string id, DeviceAuthModel updatedModel)
        {
            await _collection.ReplaceOneAsync(x => x.Id == id, updatedModel);
        }

        public async Task RemoveAsync(string id) =>
            await _collection.DeleteOneAsync(x => x.Id == id);
    }
}