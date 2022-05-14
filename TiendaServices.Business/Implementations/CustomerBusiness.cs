using System;
using System.Threading.Tasks;
using TiendaServices.Business.Intarfaces;
using TiendaServices.Common.Exceptions;
using TiendaServices.DataAccess.Interfaces;
using TiendaServices.Entities;

namespace TiendaServices.Business.Implementations
{
    public class CustomerBusiness : ICustomerBusiness
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerBusiness(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<bool> CreateCustomerAsync(Customer customer)
        {
            if (customer == null)
            {
                throw new UserInvalidException("The customer is null");
            }
            if (string.IsNullOrEmpty(customer.Name))
            {
                throw new UserInvalidException("The name customer is null");
            }
            if (string.IsNullOrEmpty(customer.DocumentNumber))
            {
                throw new UserInvalidException("The document number customer is null");
            }
            if (string.IsNullOrEmpty(customer.Email))
            {
                throw new UserInvalidException("The email customer is null");
            }
            if (string.IsNullOrEmpty(customer.Phone))
            {
                throw new UserInvalidException("The phone customer is null");
            }
            if ((DateTime.Now.Year - customer.DateBirth.Year) < 18)
            {
                throw new UserInvalidException("The customer is a minor");
            }
            customer.Active = true;

            var result = await _customerRepository.CreateAsync(customer);
            return result;
        }
    }
}