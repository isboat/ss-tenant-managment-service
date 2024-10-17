using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq.Expressions;
using Tenancy.Management.Models;
using Tenancy.Management.Mongo.Interfaces;

namespace Tenancy.Management.Mongo
{
    public class PartnerRepository : IRepository<PartnerModel>
    {
        private readonly IMongoCollection<PartnerModel> _collection;
        private readonly MongoClient _client;

        public PartnerRepository(IOptions<MongoSettings> settings)
        {
            _client = new MongoClient(
            settings.Value.ConnectionString);

            var mongoDatabase = _client.GetDatabase("TenantAdmin");

            _collection = mongoDatabase.GetCollection<PartnerModel>("Partners");
        }

        public async Task<List<PartnerModel>> GetAsync() =>
            await _collection.Find(_ => true).ToListAsync();

        public async Task<PartnerModel?> GetAsync(string id) =>
            await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();


        public async Task CreateAsync(PartnerModel newTenant) =>
            await _collection.InsertOneAsync(newTenant);

        public async Task UpdateAsync(string id, PartnerModel updatedBook) =>
            await _collection.ReplaceOneAsync(x => x.Id == id, updatedBook);

        public async Task RemoveAsync(string id) =>
            await _collection.DeleteOneAsync(x => x.Id == id);

        public IEnumerable<PartnerModel> GetByFilter(Func<PartnerModel, bool> filter)
        {
            var filtered = _collection.AsQueryable().Where(filter);
            if (filtered == null) return Enumerable.Empty<PartnerModel>();

            return filtered;
        }
    }
}