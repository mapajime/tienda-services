using System.Threading.Tasks;
using TiendaServices.Common.Data;
using TiendaServices.Entities;

namespace TiendaServices.DataAccess.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<Category> GetCategoryByNameAsync(string name);
    }
}