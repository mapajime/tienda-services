using System.Threading.Tasks;
using TiendaServices.Entities;

namespace TiendaServices.Business.Intarfaces
{
    public interface ICustomerBusiness
    {
        Task<bool> CreateCustomerAsync(Customer customer);
    }
}