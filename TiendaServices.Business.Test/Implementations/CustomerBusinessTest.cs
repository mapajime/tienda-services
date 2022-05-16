using Moq;
using System;
using System.Threading.Tasks;
using TiendaServices.Business.Implementations;
using TiendaServices.Common.Exceptions;
using TiendaServices.DataAccess.Interfaces;
using TiendaServices.Entities;
using Xunit;

namespace TiendaServices.Business.Tests.Implementations
{
    public class CustomerBusinessTest
    {
        private readonly Mock<ICustomerRepository> _mockCustomerRepository;

        public CustomerBusinessTest()
        {
            _mockCustomerRepository = new Mock<ICustomerRepository>();
        }

        [Fact]
        public async Task CreateCustomerAsync_WhenCustomerIsNull_ShouldThrowUserInvalidException()
        {
            var customer = new CustomerBusiness(_mockCustomerRepository.Object);
            var exception = await Assert.ThrowsAsync<UserInvalidException>(() => customer.CreateCustomerAsync(null));

            Assert.Contains("The customer is null", exception.Message);
            _mockCustomerRepository.Verify(c => c.CreateAsync(It.IsAny<Customer>()), Times.Never);
        }

        [Fact]
        public async Task CreateCustomerAsync_WhenNameCustomerIsNull_ShouldThrowUserInvalidException()
        {
            var customer = new CustomerBusiness(_mockCustomerRepository.Object);
            var exception = await Assert.ThrowsAsync<UserInvalidException>(() => customer.CreateCustomerAsync(new Customer { Name = null, Email = "majsh@gmail.com", Phone = "32456789", CreationDate = DateTime.Now }));
            Assert.Contains("The name customer is null", exception.Message);
            _mockCustomerRepository.Verify(c => c.CreateAsync(It.IsAny<Customer>()), Times.Never);
        }

        [Fact]
        public async Task CreateCustomerAsync_WhenDocumentNumberCustomerIsNull_ShouldThrowUserInvalidException()
        {
            var customer = new CustomerBusiness(_mockCustomerRepository.Object);
            var exception = await Assert.ThrowsAsync<UserInvalidException>(() => customer.CreateCustomerAsync(new Customer { Name = "Gumercinda Lopez", DocumentNumber = null, Email = "jdsjas@gmail.com", Phone = "3456789098" }));
            Assert.Contains("The document number customer is null", exception.Message);
            _mockCustomerRepository.Verify(c => c.CreateAsync(It.IsAny<Customer>()), Times.Never);
        }

        [Fact]
        public async Task CreateCustomerAsync_WhenEmailCustomerIsNull_ShouldThrowUserInvalidException()
        {
            var customer = new CustomerBusiness(_mockCustomerRepository.Object);
            var exception = await Assert.ThrowsAsync<UserInvalidException>(() => customer.CreateCustomerAsync(new Customer { Name = "Ana Maria", DocumentNumber = "2300567", Email = null, Phone = "231456789" }));
            Assert.Contains("The email customer is null", exception.Message);
            _mockCustomerRepository.Verify(c => c.CreateAsync(It.IsAny<Customer>()), Times.Never);
        }

        [Fact]
        public async Task CreateCustomerAsync_WhenPhoneCustomerIsNull_ShouldThrowUserInvalidException()
        {
            var customer = new CustomerBusiness(_mockCustomerRepository.Object);
            var exception = await Assert.ThrowsAsync<UserInvalidException>(() => customer.CreateCustomerAsync(new Customer { Name = "Maria Jose", DocumentNumber = "23211567", Email = "werty@gmail.com", Phone = null }));
            Assert.Contains("The phone customer is null", exception.Message);
            _mockCustomerRepository.Verify(c => c.CreateAsync(It.IsAny<Customer>()), Times.Never);
        }

        [Fact]
        public async Task CreateCustomerAsync_WhenCustomerIsYounger_ShouldThrowUserInvalidException()
        {
            var customer = new CustomerBusiness(_mockCustomerRepository.Object);
            var exception = await Assert.ThrowsAsync<UserInvalidException>(() => customer.CreateCustomerAsync(new Customer { Name = "Ana Paula", DocumentNumber = "20944567", Email = "mjhdh@hotmail.com", Phone = "231456789", DateBirth = new DateTime(2015, 12, 1) }));
            Assert.Contains("The customer is younger", exception.Message);
            _mockCustomerRepository.Verify(c => c.CreateAsync(It.IsAny<Customer>()), Times.Never);
        }
    }
}