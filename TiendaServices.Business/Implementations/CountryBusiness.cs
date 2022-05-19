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
    public class CountryBusiness : ICountryBusiness
    {
        private readonly ICountryRepository _countryRepository;

        public CountryBusiness(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public async Task<bool> CreateCountryAsync(Country country)
        {
            Validate(country);
            var isCountry = await _countryRepository.CreateAsync(country);
            return isCountry;
        }

        public async Task<bool> DeleteCountryAsync(Guid id)
        {
            var isDelete = await _countryRepository.DeleteAsync(id);
            return isDelete;
        }

        public async Task<IEnumerable<Country>> GetAllCountryAsync()
        {
            var countries = (await _countryRepository.GetAllAsync()).ToList();
            return countries;
        }

        public async Task<Country> GetCountryByIdAsync(Guid id)
        {
            var country = await _countryRepository.GetByIdAsync(id);
            return country;
        }

        public async Task<bool> UpdateCountryAsync(Country country)
        {
            Validate(country);
            var isUpdateCountry = await _countryRepository.UpdateAsync(country);
            return isUpdateCountry;
        }

        private void Validate(Country country)
        {
            if (country == null)
            {
                throw new CityInvalidException("The country is null");
            }
            if (string.IsNullOrEmpty(country.Name))
            {
                throw new CityInvalidException("The country is empty or null");
            }
            if (_countryRepository.GetCountryByNameAsync(country.Name) != null)
            {
                throw new CityInvalidException("The country name already exists");
            }
        }
    }
}