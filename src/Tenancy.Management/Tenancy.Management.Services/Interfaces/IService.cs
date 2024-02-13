using System.Linq.Expressions;
using Tenancy.Management.Models;

namespace Tenancy.Management.Services.Interfaces
{
    public interface IService<T>
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<IEnumerable<T>> GetByFilterAsync(Expression<Func<DeviceAuthModel, bool>> filter);

        Task<T?> GetAsync(string id);

        public Task UpdateAsync(string id, T updatedModel);

        public Task RemoveAsync(string id);
    }
}
