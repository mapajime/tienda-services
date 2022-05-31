using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TiendaServices.Business.Intarfaces;
using TiendaServices.Common.Exceptions;
using TiendaServices.DataAccess.Interfaces;
using TiendaServices.Entities;

namespace TiendaServices.Business.Implementations
{
    public class ProductBusiness : IProductBusiness
    {
        private readonly IProductRepository _productRepository;

        public ProductBusiness(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<bool> CreateProductAsync(Product product)
        {
            await ValidateAsync(product);
            product.Active = true;
            var isProduct = await _productRepository.CreateAsync(product);
            return isProduct;
        }

        public async Task<IEnumerable<Product>> GetAllProductAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return products;
        }

        public async Task<IEnumerable<Product>> GetAllActiveProducts()
        {
            var products = await _productRepository.GetAllActiveProducts();
            return products;
        }

        public async Task<IEnumerable<Product>> GetAllActiveProductsByCategory(Guid idCategory)
        {
            var products = await _productRepository.GetAllActiveProductsByCategory(idCategory);
            return products;
        }

        public async Task<Product> GetProductByIdAsync(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            return product;
        }

        public async Task<bool> DeactivateProduct(Guid id)
        {
            var product = await _productRepository.DeactivateProduct(id);
            return product;
        }

        public async Task<IEnumerable<Product>> GetProductsThatMatchNameAndAreActive(string name)
        {
            var products = await _productRepository.GetProductsThatMatchNameAndAreActive(name);
            return products;
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            await ValidateAsync(product);
            var isUpdateProduct = await _productRepository.UpdateAsync(product);
            return isUpdateProduct;
        }

        private async Task ValidateAsync(Product product)
        {
            if (product == null)
            {
                throw new ProductInvalidException("The Product is null");
            }
            if (string.IsNullOrEmpty(product.ProductName))
            {
                throw new ProductInvalidException("The Product is empty or null");
            }
            if (product.Price <= 0)
            {
                throw new ProductInvalidException("The price is negative");
            }
            if (string.IsNullOrEmpty(product.Trademark))
            {
                throw new ProductInvalidException("The trademark product is null");
            }
            if (product.Stock <= 0)
            {
                throw new ProductInvalidException("The stock is negative");
            }
        }
    }
}