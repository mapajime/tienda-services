using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TiendaServices.Business.Intarfaces;
using TiendaServices.Common.Exceptions;
using TiendaServices.DataAccess.Interfaces;
using TiendaServices.Entities;

namespace TiendaServices.Business.Implementations
{
    public class CityBusiness : ICityBusiness
    {
        private readonly ICityRepository _cityRepository;

        public CityBusiness(ICityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }

        public async Task<bool> CreateCityAsync(City city)
        {
            await ValidateAsync(city);
            var isCity = await _cityRepository.CreateAsync(city);
            return isCity;
        }

        public async Task<bool> DeleteCityAsync(Guid id)
        {
            var isDelete = await _cityRepository.DeleteAsync(id);
            return isDelete;
        }

        public async Task<IEnumerable<City>> GetAllCityAsync()
        {
            var cities = (await _cityRepository.GetAllAsync()).ToList();
            return cities;
        }

        public async Task<City> GetCityByIdAsync(Guid id)
        {
            var city = await _cityRepository.GetByIdAsync(id);
            return city;
        }

        public async Task<bool> UpdateCityAsync(City city)
        {
            await ValidateAsync(city);
            var isUpdateCity = await _cityRepository.UpdateAsync(city);
            return isUpdateCity;
        }

        private async Task ValidateAsync(City city)
        {
            if (city == null)
            {
                throw new CityInvalidException("The City is null");
            }
            if (string.IsNullOrEmpty(city.Name))
            {
                throw new CityInvalidException("The City is empty or null");
            }
            if (await _cityRepository.GetByNameAsync(city.Name) != null)
            {
                throw new CityInvalidException("The City name already exists");
            }
        }
    }
}