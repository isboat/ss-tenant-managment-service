using System.Linq.Expressions;
using Tenancy.Management.Models;
using Tenancy.Management.Mongo.Interfaces;
using Tenancy.Management.Services.Interfaces;

namespace Tenancy.Management.Services
{
    public class BaseService<T> : IService<T> where T : class
    {
        private readonly IRepository<T> _repository;

        public BaseService(IRepository<T> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _repository.GetAsync();
        }

        public IEnumerable<T> GetByFilter(Func<T, bool> filter)
        {
            return _repository.GetByFilter(filter);
        }

        public async Task<T?> GetAsync(string id)
        {
            return await _repository.GetAsync(id);
        }

        public async Task UpdateAsync(string id, T updatedModel)
        {
            await _repository.UpdateAsync(id, updatedModel);
        }

        public async Task RemoveAsync(string id)
        {
            await _repository.RemoveAsync(id);
        }

        public virtual async Task CreateAsync(T model)
        {
            await _repository.CreateAsync(model);
        }
    }
}