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
    public class ProductBusinessTest
    {
        private readonly Mock<IProductRepository> _mockProductRepository;

        public ProductBusinessTest()
        {
            _mockProductRepository = new Mock<IProductRepository>();
        }

        [Fact]
        public async Task CreateProductAsync_WhenProductIsNull_ShouldThrowProductInvalidException()
        {
            var productBusiness = new ProductBusiness(_mockProductRepository.Object);
            var exception = await Assert.ThrowsAsync<ProductInvalidException>(() => productBusiness.CreateProductAsync(null));
            Assert.Contains("The Product is null", exception.Message);
            _mockProductRepository.Verify(c => c.CreateAsync(It.IsAny<Product>()), Times.Never);
        }

        [Fact]
        public async Task CreateProductAsync_WhenProductIsEmpty_ShouldThrowProductInvalidException()
        {
            var productBusiness = new ProductBusiness(_mockProductRepository.Object);
            var exception = await Assert.ThrowsAsync<ProductInvalidException>(() => productBusiness.CreateProductAsync(new Product { ProductName = string.Empty, Active = true, Price = 234000, Stock = 2, Description = string.Empty }));
            Assert.Contains("The Product is empty or null", exception.Message);
            _mockProductRepository.Verify(c => c.CreateAsync(It.IsAny<Product>()), Times.Never);
        }

        [Fact]
        public async Task CreateProductAsync_WhenPriceOfTheProductIsNegative_ShouldThrowProductInvalidException()
        {
            var productBusiness = new ProductBusiness(_mockProductRepository.Object);
            var exception = await Assert.ThrowsAsync<ProductInvalidException>(() => productBusiness.CreateProductAsync(new Product { ProductName = "Arroz", Active = true, Price = -1, Stock = 2, Description = "Product is valid" }));
            Assert.Contains("The price is negative", exception.Message);
            _mockProductRepository.Verify(c => c.CreateAsync(It.IsAny<Product>()), Times.Never);
        }

        [Fact]
        public async Task CreateProductAsync_WhenTrademarkIsEmpty_ShouldThrowProductInvalidException()
        {
            var productBusiness = new ProductBusiness(_mockProductRepository.Object);
            var exception = await Assert.ThrowsAsync<ProductInvalidException>(() => productBusiness.CreateProductAsync(new Product { ProductName = "Arroz", Active = true, Trademark = string.Empty, Price = 123566, Stock = 2, Description = "Product is valid" }));
            Assert.Contains("The trademark product is nul", exception.Message);
            _mockProductRepository.Verify(c => c.CreateAsync(It.IsAny<Product>()), Times.Never);
        }

        [Fact]
        public async Task CreateProductAsync_WhenStockOfTheProductIsNegative_ShouldThrowProductInvalidException()
        {
            var productBusiness = new ProductBusiness(_mockProductRepository.Object);
            var exception = await Assert.ThrowsAsync<ProductInvalidException>(() => productBusiness.CreateProductAsync(
                new Product
                {
                    ProductName = "Arroz",
                    Active = true,
                    Trademark = "Roa",
                    Price = 2311,
                    Stock = -1,
                    Description = "Product is valid"
                }));
            Assert.Contains("The stock is negative", exception.Message);
            _mockProductRepository.Verify(c => c.CreateAsync(It.IsAny<Product>()), Times.Never);
        }

        [Fact]
        public async Task CreateProductAsync_WhenProductIsOk_ShouldReturnTrue()
        {
            Product product = null;
            _mockProductRepository.Setup(p => p.CreateAsync(It.IsAny<Product>()))
                .Callback<Product>(p => product = p)
                .ReturnsAsync(true);
            var productBusiness = new ProductBusiness(_mockProductRepository.Object);
            var result = await productBusiness.CreateProductAsync(new Product { ProductName = "Harina", Price = 12345, Stock = 10, Active = true, Trademark = "Haz de oro", Description = "Producto cocina", CreationDate = new DateTime(2022 / 1 / 2), ModificationDate = new DateTime(2022 / 04 / 03), FKCategory = Guid.Empty });
            Assert.True(result);
            Assert.NotNull(product);
            Assert.True(product.Active);
            Assert.Equal("Harina", product.ProductName);
            Assert.Equal("Haz de oro", product.Trademark);
            _mockProductRepository.Verify(c => c.CreateAsync(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public async Task DeactivateProduct_WhenIdProductoIsValid_ShouldDeactivateProduct()
        {
            _mockProductRepository.Setup(c => c.DeactivateProduct(It.IsAny<Guid>()))
                .ReturnsAsync(true);
            var productBusiness = new ProductBusiness(_mockProductRepository.Object);
            var result = await productBusiness.DeactivateProduct(Guid.Empty);
            Assert.True(result);
            _mockProductRepository.Verify(c => c.DeactivateProduct(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task GetProductByIdAsync_WhenIdProductExist_ShouldReturnProduct()
        {
            _mockProductRepository.Setup(c => c.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(
                new Product
                {
                    ProductName = "Queso crema",
                    Price = 5678,
                    Stock = 12,
                    Active = true,
                    Description = "Lacteos",
                    Trademark = "Alpina",
                    CreationDate = new DateTime(2022, 1, 3),
                    ModificationDate = new DateTime(2022, 5, 3),
                    FKCategory = Guid.Empty
                }); ;
            var productBusiness = new ProductBusiness(_mockProductRepository.Object);
            var result = await productBusiness.GetProductByIdAsync(Guid.Empty);
            Assert.NotNull(result);
            Assert.Equal("Queso crema", result.ProductName);
            _mockProductRepository.Verify(c => c.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task GetAllActiveProductsByCategory_WhenIdCategoryExistAndActiveIsTrue_ShouldReturnProducts()
        {
            _mockProductRepository.Setup(c => c.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(
               new Product
               {
                   ProductName = "Mani",
                   Price = 1278,
                   Stock = 3,
                   Active = true,
                   Description = "Granos",
                   Trademark = "Tosh",
                   CreationDate = new DateTime(2021, 1, 3),
                   ModificationDate = new DateTime(2022, 1, 3),
                   FKCategory = Guid.Empty
               }); ;
            var productBusiness = new ProductBusiness(_mockProductRepository.Object);
            var result = await productBusiness.GetAllActiveProductsByCategory(Guid.Empty);
            Assert.NotNull(result);

            _mockProductRepository.Verify(c => c.GetAllActiveProductsByCategory(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task GetAllProductAsync_WhenProductsExists_ShouldReturnAllProducts()
        {
            _mockProductRepository.Setup(c => c.GetAllAsync()).ReturnsAsync(
                new List<Product>
                {
                    new Product
                    {
                       ProductName = "Granola",
                       Price = 34278,
                       Stock = 1,
                       Active = true,
                       Description = "Cereal",
                       Trademark = "Tosh",
                       CreationDate = new DateTime(2019 ,1, 3),
                       ModificationDate = new DateTime(2020, 1, 3),
                       FKCategory = Guid.Empty
                    },
                    new Product
                    {
                       ProductName = "Almendras",
                       Price = 8278,
                       Stock = 4,
                       Active = true,
                       Description = "Cereal",
                       Trademark = "Nuthos",
                       CreationDate = new DateTime(2022, 1,3),
                       ModificationDate = new DateTime(2022, 16, 30),
                       FKCategory = Guid.Empty
                    },
                       new Product
                    {
                       ProductName = "Uvas pasas",
                       Price = 5278,
                       Stock = 4,
                       Active = true,
                       Description = "Cereal",
                       Trademark = "Nuthos",
                       CreationDate = new DateTime(2022, 1, 3),
                       ModificationDate = new DateTime(2022, 16, 30),
                       FKCategory = Guid.Empty
                    }
                });
            var productcityBusiness = new ProductBusiness(_mockProductRepository.Object);
            var products = await productcityBusiness.GetAllProductAsync();
            Assert.NotNull(products);
            Assert.Equal("Granola", products.First().ProductName);
            Assert.Equal(3, products.Count());
        }

        [Fact]
        public async Task GetAllActiveProducts_WhenActiveIsTrue_ShouldReturnProducts()
        {
            _mockProductRepository.Setup(c => c.GetAllActiveProducts())
                .ReturnsAsync(
             new List<Product>
             {
                    new Product
                    {
                       ProductName = "Almendras",
                       Price = 8278,
                       Stock = 4,
                       Active = true,
                       Description = "Cereal",
                       Trademark = "Nutho",
                       FKCategory = Guid.Empty
                    },
                      new Product
                    {
                       ProductName = "Ciruelas",
                       Price = 8078,
                       Stock = 2,
                       Active = false,
                       Description = "Cereal",
                       Trademark = "Nuthos",
                       FKCategory = Guid.Empty
                    }
             });
            var productBusiness = new ProductBusiness(_mockProductRepository.Object);
            var result = (await productBusiness.GetAllActiveProducts()).ToList();
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains("Ciruelas", result.Last().ProductName);
            Assert.Equal("Nuthos", result[1].Trademark);

            _mockProductRepository.Verify(c => c.GetAllActiveProducts(), Times.Once);
        }

        [Fact]
        public async Task GetProductsThatMatchNameAndAreActive_WhenProductsThatMatchNameAndAreActive_ShouldReturnProducts()
        {
            string productName = null;
            _mockProductRepository.Setup(p => p.GetProductsThatMatchNameAndAreActive(It.IsAny<string>()))
                .Callback<string>(p => productName = p)
                .ReturnsAsync(
                    new List<Product> {
                    new Product
                    {
                        ProductName = "Granola",
                        Price = 34278,
                        Stock = 1,
                        Active = true,
                        Description = "Cereal",
                        Trademark = "Tosh",
                        CreationDate = new DateTime(2019,12,3),
                        ModificationDate = new DateTime(2020,3,4),
                        FKCategory = Guid.Empty
                    }});
            var productBusiness = new ProductBusiness(_mockProductRepository.Object);
            var result = await productBusiness.GetProductsThatMatchNameAndAreActive("Granola");
            Assert.NotNull(result);
            Assert.Equal("Granola", productName);
            Assert.Single(result);
            Assert.Contains(result, p => p.Description == "Cereal");
            Assert.Equal("Tosh", result.First().Trademark);

            _mockProductRepository.Verify(c => c.GetProductsThatMatchNameAndAreActive(It.IsAny<string>()), Times.Once);
        }
    }
}