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
    public class CityBusinessTest
    {
        public readonly Mock<ICityRepository> _mockCityRepository;

        public CityBusinessTest()
        {
            _mockCityRepository = new Mock<ICityRepository>();
        }

        [Fact]
        public async Task CreateCityAsync_WhenCityIsNull_ShouldThrowCityInvalidException()
        {
            var cityBusiness = new CityBusiness(_mockCityRepository.Object);
            var exception = await Assert.ThrowsAsync<CityInvalidException>(() => cityBusiness.CreateCityAsync(null));
            Assert.Contains("The City is null", exception.Message);
            _mockCityRepository.Verify(c => c.CreateAsync(It.IsAny<City>()), Times.Never);
        }

        [Fact]
        public async Task CreateCityAsync_WhenCityIsEmpty_ShouldThrowCityInvalidException()
        {
            var cityBusiness = new CityBusiness(_mockCityRepository.Object);
            var exception = await Assert.ThrowsAsync<CityInvalidException>(() => cityBusiness.CreateCityAsync(new City { Name = string.Empty, Description = string.Empty }));
            Assert.Contains("The City is empty or null", exception.Message);
            _mockCityRepository.Verify(c => c.CreateAsync(It.IsAny<City>()), Times.Never);
        }

        [Fact]
        public async Task CreateCityAsync_WhenCityAlreadyExists_ShouldThrowCityInvalidException()
        {
            _mockCityRepository.Setup(c => c.GetByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new City { Name = "Bogota", Description = "Ciudad capital" });

            var cityBusiness = new CityBusiness(_mockCityRepository.Object);
            var exception = await Assert.ThrowsAnyAsync<CityInvalidException>(() => cityBusiness.CreateCityAsync(new City { Name = "Bogota", Description = "Ciudad capital" }));
            Assert.Contains("The City name already exists", exception.Message);
        }

        [Fact]
        public async Task CreateCityAsync_WhenCityIsOk_ShouldReturnTrue()
        {
            City city = null;
            _mockCityRepository.Setup(c => c.GetByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((City)null);
            _mockCityRepository.Setup(c => c.CreateAsync(It.IsAny<City>()))
                .Callback<City>(c => city = c)
                .ReturnsAsync(true);
            var cityBusiness = new CityBusiness(_mockCityRepository.Object);
            var result = await cityBusiness.CreateCityAsync(new City { Name = "Medellin", Description = "Es la capital de Antioquia" });
            Assert.True(result);
            Assert.NotNull(city.Name);
            Assert.Equal("Medellin", city.Name);
            _mockCityRepository.Verify(c => c.CreateAsync(It.IsAny<City>()), Times.Once);
        }

        [Fact]
        public async Task DeleteCityAsync_WhenIdCityIsOk_ShouldReturnTrue()
        {
            _mockCityRepository.Setup(c => c.DeleteAsync(It.IsAny<Guid>()))
                .ReturnsAsync(true);
            var cityBusiness = new CityBusiness(_mockCityRepository.Object);
            var result = await cityBusiness.DeleteCityAsync(Guid.Empty);
            Assert.True(result);
            _mockCityRepository.Verify(c => c.DeleteAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task GetAllCityAsync_WhenCountriesExists_ShouldReturnAllCountries()
        {
            _mockCityRepository.Setup(c => c.GetAllAsync()).ReturnsAsync(
                new List<City>
                {
                    new City
                    {
                        Name = "Bogota",
                        Description = "Ciudad capital"
                    },
                    new City
                    {
                         Name = "Medellin",
                        Description = "Ciudad capital"
                    }
                });
            var cityBusiness = new CityBusiness(_mockCityRepository.Object);
            var countries = await cityBusiness.GetAllCityAsync();
            Assert.NotNull(countries);
            Assert.Equal("Medellin", countries.Last().Name);
            Assert.Equal(2, countries.Count());
        }

        [Fact]
        public async Task GetCityByIdAsync_WhenIdCityExist_ShouldReturnCity()
        {
            _mockCityRepository.Setup(c => c.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(
                new City
                {
                    Name = "Cartagena",
                    Description = "Ciudad de playas"
                });
            var cityBusiness = new CityBusiness(_mockCityRepository.Object);
            var result = await cityBusiness.GetCityByIdAsync(Guid.Empty);
            Assert.NotNull(result);
            Assert.Equal("Cartagena", result.Name);
            _mockCityRepository.Verify(c => c.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task UpdateCityAsync_WhenCityIsNull_ShouldThrowCityInvalidException()
        {
            var cityBusiness = new CityBusiness(_mockCityRepository.Object);
            var exception = await Assert.ThrowsAsync<CityInvalidException>(() => cityBusiness.UpdateCityAsync(null));
            Assert.Contains("The City is null", exception.Message);
            _mockCityRepository.Verify(c => c.UpdateAsync(It.IsAny<City>()), Times.Never);
        }

        [Fact]
        public async Task UpdateCityAsync_WhenCityIsEmpty_ShouldThrowCityInvalidException()
        {
            var cityBusiness = new CityBusiness(_mockCityRepository.Object);
            var exception = await Assert.ThrowsAsync<CityInvalidException>(() => cityBusiness.UpdateCityAsync(new City()));
            Assert.Contains("The City is empty or nul", exception.Message);
            _mockCityRepository.Verify(c => c.UpdateAsync(It.IsAny<City>()), Times.Never);
        }

        [Fact]
        public async Task UpdateCityAsync_WhenCityAlreadyExist_ShouldThrowCityInvalidException()
        {
            _mockCityRepository.Setup(c => c.GetByNameAsync(It.IsAny<string>())).ReturnsAsync(new City());
            var cityBusiness = new CityBusiness(_mockCityRepository.Object);
            var exception = await Assert.ThrowsAsync<CityInvalidException>(() => cityBusiness.UpdateCityAsync(new City
            {
                Name = "Bogota",
                Description = "Prueba"
            }));
            Assert.Contains("The City name already exists", exception.Message);
            _mockCityRepository.Verify(c => c.UpdateAsync(It.IsAny<City>()), Times.Never);
        }

        [Fact]
        public async Task UpdateCityAsync_WhenCityIsOk_ShouldReturnTrue()
        {
            _mockCityRepository.Setup(c => c.GetByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((City)null);
            _mockCityRepository.Setup(c => c.UpdateAsync(It.IsAny<City>())).ReturnsAsync(true);
            var cityBusiness = new CityBusiness(_mockCityRepository.Object);
            var isCity = await cityBusiness.UpdateCityAsync(new City
            {
                Name = "Cartagena",
                Description = "Prueba",
                CreationDate = new DateTime(2020, 12, 3),
                ModificationDate = new DateTime(2021, 2, 4)
            });
            Assert.True(isCity);
            _mockCityRepository.Verify(c => c.UpdateAsync(It.IsAny<City>()), Times.Once);
        }
    }
}