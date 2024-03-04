using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tenancy.Management.Models;

namespace Tenancy.Management.Mongo.Interfaces
{
    public interface IRepository<T>
    {
        public Task<List<T>> GetAsync();

        public Task<IEnumerable<T>> GetByFilterAsync(Expression<Func<T, bool>> filter);

        public Task<T?> GetAsync(string id);

        public Task CreateAsync(T newModel);

        public Task UpdateAsync(string id, T updatedModel);

        public Task RemoveAsync(string id);
    }
}
