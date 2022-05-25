using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TiendaServices.Entities;

namespace TiendaServices.Business.Intarfaces
{
    public interface IProductBusiness
    {
        Task<bool> CreateProductAsync(Product product);

        Task<bool> UpdateProductAsync(Product product);

        Task<bool> DeactivateProduct(Guid id);

        Task<IEnumerable<Product>> GetAllActiveProductsByCategory(Guid idCategory);

        Task<IEnumerable<Product>> GetAllActiveProducts();

        Task<IEnumerable<Product>> GetAllProductAsync();

        Task<Product> GetProductByIdAsync(Guid id);

        Task<IEnumerable<Product>> GetProductsThatMatchNameAndAreActive(string name);
    }
}