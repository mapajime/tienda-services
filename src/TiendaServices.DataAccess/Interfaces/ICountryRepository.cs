using System.Threading.Tasks;
using TiendaServices.Common.Data;
using TiendaServices.Entities;

namespace TiendaServices.DataAccess.Interfaces
{
    public interface ICountryRepository : IRepository<Country>
    {
        Task<Country> GetCountryByNameAsync(string name);
    }
}