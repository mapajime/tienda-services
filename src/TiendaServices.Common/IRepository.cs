using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TiendaServices.Common
{
    public interface IRepository<T> where T : IEntity
    {
        Task<T> CreateAsync(T value);

        Task<bool> UpdateAsync(T value);

        Task DeleteAsync(Guid id);

        Task<int> GetCountAsync();

        Task<IEnumerable<T>> GetAllAsync();

        Task<T> GetByIdAsync(Guid id);
    }
}