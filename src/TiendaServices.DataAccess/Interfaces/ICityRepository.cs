using System.Threading.Tasks;
using TiendaServices.Common.Data;
using TiendaServices.Entities;

namespace TiendaServices.DataAccess.Interfaces
{
    public interface ICityRepository : IRepository<City>
    {
        Task<City> GetByNameAsync(string name);
    }
}