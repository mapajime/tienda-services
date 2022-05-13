using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TiendaServices.Common
{
    public interface IRepository<TEntity> where TEntity : IEntity
    {
        Task<bool> CreateAsync(TEntity value);

        Task<bool> UpdateAsync(TEntity value);

        Task<bool> DeleteAsync(Guid id);

        Task<int> GetCountAsync();

        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<TEntity> GetByIdAsync(Guid id);
    }
}