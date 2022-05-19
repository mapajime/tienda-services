using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TiendaServices.Entities;

namespace TiendaServices.Business.Intarfaces
{
    public interface ICountryBusiness
    {
        Task<bool> CreateCountryAsync(Country country);

        Task<bool> UpdateCountryAsync(Country country);

        Task<bool> DeleteCountryAsync(Guid id);

        Task<Country> GetCountryByIdAsync(Guid id);

        Task<IEnumerable<Country>> GetAllCountryAsync();
    }
}