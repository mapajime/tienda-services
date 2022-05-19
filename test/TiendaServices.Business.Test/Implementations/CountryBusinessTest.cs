using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TiendaServices.Business.Implementations;
using TiendaServices.Common.Exceptions;
using TiendaServices.DataAccess.Interfaces;
using TiendaServices.Entities;
using Xunit;

namespace TiendaServices.Business.Tests.Implementations
{
    public class CountryBusinessTest
    {
        public readonly Mock<ICountryRepository> _mockCountryRepository;

        public CountryBusinessTest()
        {
            _mockCountryRepository = new Mock<ICountryRepository>();
        }

        [Fact]
        public async Task CreateCountryAsync_WhenCountryIsNull_ShouldThrowCityInvalidException()
        {
            var countryBusiness = new CountryBusiness(_mockCountryRepository.Object);
            var exception = await Assert.ThrowsAsync<CityInvalidException>(() => countryBusiness.CreateCountryAsync(null));
            Assert.Contains("The country is null", exception.Message);
            _mockCountryRepository.Verify(c => c.CreateAsync(It.IsAny<Country>()), Times.Never);
        }

        [Fact]
        public async Task CreateCountryAsync_WhenCountryIsEmpty_ShouldThrowCityInvalidException()
        {
            var countryBusiness = new CountryBusiness(_mockCountryRepository.Object);
            var exception = await Assert.ThrowsAsync<CityInvalidException>(() => countryBusiness.CreateCountryAsync(new Country { Name = string.Empty, Description = string.Empty }));
            Assert.Contains("The country is empty or null", exception.Message);
            _mockCountryRepository.Verify(c => c.CreateAsync(It.IsAny<Country>()), Times.Never);
        }

        [Fact]
        public async Task CreateCountryAsync_WhenCountryAlreadyExists_ShouldThrowCityInvalidException()
        {
            _mockCountryRepository.Setup(c => c.GetCountryByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new Country { Name = "Bogota", Description = "Ciudad capital" });

            var countryBusiness = new CountryBusiness(_mockCountryRepository.Object);
            var exception = await Assert.ThrowsAnyAsync<CityInvalidException>(() => countryBusiness.CreateCountryAsync(new Country { Name = "Bogota", Description = "Ciudad capital" }));
            Assert.Contains("The country name already exists", exception.Message);
        }

        [Fact]
        public async Task CreateCountryAsync_WhenCountryIsOk_ShouldReturnTrue()
        {
            Country country = null;
            _mockCountryRepository.Setup(c => c.GetCountryByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((Country)null);
            _mockCountryRepository.Setup(c => c.CreateAsync(It.IsAny<Country>()))
                .Callback<Country>(c => country = c)
                .ReturnsAsync(true);
            var countryBusiness = new CountryBusiness(_mockCountryRepository.Object);
            var result = await countryBusiness.CreateCountryAsync(new Country { Name = "Medellin", Description = "Es la capital de Antioquia" });
            Assert.True(result);
            Assert.NotNull(country.Name);
            Assert.Equal("Medellin", country.Name);
            _mockCountryRepository.Verify(c => c.CreateAsync(It.IsAny<Country>()), Times.Once);
        }

        [Fact]
        public async Task DeleteCountryAsync_WhenIdCountryIsOk_ShouldReturnTrue()
        {
            _mockCountryRepository.Setup(c => c.DeleteAsync(It.IsAny<Guid>()))
                .ReturnsAsync(true);
            var countryBusiness = new CountryBusiness(_mockCountryRepository.Object);
            var result = await countryBusiness.DeleteCountryAsync(Guid.Empty);
            Assert.True(result);
            _mockCountryRepository.Verify(c => c.DeleteAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task GetAllCountryAsync_WhenCountriesExists_ShouldReturnAllCountries()
        {
            _mockCountryRepository.Setup(c => c.GetAllAsync()).ReturnsAsync(
                new List<Country>
                {
                    new Country
                    {
                        Name = "Bogota",
                        Description = "Ciudad capital"
                    },
                    new Country
                    {
                         Name = "Medellin",
                        Description = "Ciudad capital"
                    }
                });
            var countryBusiness = new CountryBusiness(_mockCountryRepository.Object);
            var countries = await countryBusiness.GetAllCountryAsync();
            Assert.NotNull(countries);
            Assert.Equal("Medellin", countries.Last().Name);
            Assert.Equal(2, countries.Count());
            _mockCountryRepository.Verify(c => c.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task GetCountryByIdAsync_WhenIdCountryExist_ShouldReturnCountry()
        {
            _mockCountryRepository.Setup(c => c.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(
                new Country
                {
                    Name = "Cartagena",
                    Description = "Ciudad de playas"
                });
            var countryBusiness = new CountryBusiness(_mockCountryRepository.Object);
            var result = await countryBusiness.GetCountryByIdAsync(Guid.Empty);
            Assert.NotNull(result);
            Assert.Equal("Cartagena", result.Name);
            _mockCountryRepository.Verify(c => c.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task UpdateCountryAsync_WhenCountryIsNull_ShouldThrowCityInvalidException()
        {
            var countryBusiness = new CountryBusiness(_mockCountryRepository.Object);
            var exception = await Assert.ThrowsAsync<CityInvalidException>(() => countryBusiness.UpdateCountryAsync(null));
            Assert.Contains("The country is null", exception.Message);
            _mockCountryRepository.Verify(c => c.UpdateAsync(It.IsAny<Country>()), Times.Never);
        }

        [Fact]
        public async Task UpdateCountryAsync_WhenCountryIsEmpty_ShouldThrowCityInvalidException()
        {
            var countryBusiness = new CountryBusiness(_mockCountryRepository.Object);
            var exception = await Assert.ThrowsAsync<CityInvalidException>(() => countryBusiness.UpdateCountryAsync(new Country()));
            Assert.Contains("The country is empty or nul", exception.Message);
            _mockCountryRepository.Verify(c => c.UpdateAsync(It.IsAny<Country>()), Times.Never);
        }

        [Fact]
        public async Task UpdateCountryAsync_WhenCountryAlreadyExist_ShouldThrowCityInvalidException()
        {
            _mockCountryRepository.Setup(c => c.GetCountryByNameAsync(It.IsAny<string>())).ReturnsAsync(new Country());
            var countryBusiness = new CountryBusiness(_mockCountryRepository.Object);
            var exception = await Assert.ThrowsAsync<CityInvalidException>(() => countryBusiness.UpdateCountryAsync(new Country
            {
                Name = "Bogota",
                Description = "Prueba"
            }));
            Assert.Contains("The country name already exists", exception.Message);
            _mockCountryRepository.Verify(c => c.UpdateAsync(It.IsAny<Country>()), Times.Never);
        }

        [Fact]
        public async Task UpdateCountryAsync_WhenCountryIsOk_ShouldReturnTrue()
        {
            _mockCountryRepository.Setup(c => c.UpdateAsync(It.IsAny<Country>())).ReturnsAsync(true);
            var countryBusiness = new CountryBusiness(_mockCountryRepository.Object);
            var isCountry = await countryBusiness.UpdateCountryAsync(new Country
            {
                Name = "Cartagena",
                Description = "Prueba",
                CreationDate = new DateTime(2020, 12, 3),
                ModificationDate = new DateTime(2021, 2, 4)
            });
            Assert.True(isCountry);
            _mockCountryRepository.Verify(c => c.UpdateAsync(It.IsAny<Country>()), Times.Once);
        }
    }
}