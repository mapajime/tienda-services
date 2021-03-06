using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TiendaServices.Common.Data;
using TiendaServices.Entities;

namespace TiendaServices.DataAccess.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<bool> DeactivateProduct(Guid id);

        Task<IEnumerable<Product>> GetAllActiveProductsByCategory(Guid idCategory);

        Task<IEnumerable<Product>> GetAllActiveProducts();

        Task<IEnumerable<Product>> GetProductsThatMatchNameAndAreActive(string name);
    }
}