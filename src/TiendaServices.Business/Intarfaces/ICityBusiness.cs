using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TiendaServices.Entities;

namespace TiendaServices.Business.Intarfaces
{
    public interface ICityBusiness
    {
        Task<bool> CreateCityAsync(City City);

        Task<bool> UpdateCityAsync(City City);

        Task<bool> DeleteCityAsync(Guid id);

        Task<City> GetCityByIdAsync(Guid id);

        Task<IEnumerable<City>> GetAllCityAsync();
    }
}