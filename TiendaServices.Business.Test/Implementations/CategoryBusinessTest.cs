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
    public class CategoryBusinessTest
    {
        private readonly Mock<ICategoryRepository> _mockCategoryRepository;

        public CategoryBusinessTest()
        {
            _mockCategoryRepository = new Mock<ICategoryRepository>();
        }

        [Fact]
        public async Task CreateCategoryAsync_WhenCategoryIsNull_ShouldThrowCategoryInvalidException()
        {
            var categoryBusiness = new CategoryBusiness(_mockCategoryRepository.Object);
            var exception = await Assert.ThrowsAsync<CategoryInvalidException>(() => categoryBusiness.CreateCategoryAsync(null));
            Assert.Contains("The category is null", exception.Message);
            _mockCategoryRepository.Verify(c => c.CreateAsync(It.IsAny<Category>()), Times.Never);
        }

        [Fact]
        public async Task CreateCategoryAsync_WhenNameCategoryIsEmpty_ShouldThrowCategoryInvalidException()
        {
            var categoryBusiness = new CategoryBusiness(_mockCategoryRepository.Object);
            var exception = await Assert.ThrowsAsync<CategoryInvalidException>(() => categoryBusiness.CreateCategoryAsync(new Category { Name = string.Empty, Description = "Categoria prueba" }));
            Assert.Contains("The name category is empty", exception.Message);
            _mockCategoryRepository.Verify(c => c.CreateAsync(It.IsAny<Category>()), Times.Never);
        }

        [Fact]
        public async Task CreateCategoryAsync_WhenNameCategoryExists_ShouldThrowCategoryInvalidException()
        {
            Category category = null;
            _mockCategoryRepository.Setup(m => m.CreateAsync(It.IsAny<Category>()))
                .Callback<Category>(c => category = c)
                .ReturnsAsync(true);
            var categoryBusiness = new CategoryBusiness(_mockCategoryRepository.Object);
            var exception = await Assert.ThrowsAsync<CategoryInvalidException>(() => categoryBusiness.CreateCategoryAsync(new Category { Name = "Electrodomesticos", Description = "Prueba categoria" }));
            Assert.Contains("The category already exists", exception.Message);
            Assert.NotNull(category);
            Assert.Equal("Electrodomesticos", category.Name);
        }

        [Fact]
        public async Task CreateCategoryAsync_WhenNameCategoryIsOk_ShouldReturnTrue()
        {
            Category category = null;
            _mockCategoryRepository.Setup(m => m.CreateAsync(It.IsAny<Category>()))
                .Callback<Category>(c => category = c)
                .ReturnsAsync(true);
            var categoryBusiness = new CategoryBusiness(_mockCategoryRepository.Object);
            var result = await categoryBusiness.CreateCategoryAsync(new Category { Name = "Aseo", Description = "Elementos de aseo personal" });
            Assert.True(result);
            Assert.NotNull(category);
            Assert.Equal("Aseo", category.Name);
        }

        [Fact]
        public async Task DeleteCategoryAsync_WhenIdCategoryExist_ShouldDeleteCategory()
        {
            _mockCategoryRepository.Setup(c => c.DeleteAsync(It.IsAny<Guid>())).ReturnsAsync(true);

            var categoryBusiness = new CategoryBusiness(_mockCategoryRepository.Object);
            var response = await categoryBusiness.DeleteCategoryAsync(Guid.Empty);
            Assert.True(response);
            _mockCategoryRepository.Verify(c => c.DeleteAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task GetAllCategoriesAsync_WhenCategoryExist_ShouldReturnAllCategory()
        {
            _mockCategoryRepository.Setup(c => c.GetAllAsync()).ReturnsAsync(
                new List<Category>
                {
                    new Category
                    {
                    Name = "Category 1",
                    Description = "The category is toilet"
                    },
                    new Category
                    {
                        Name="Category 2",
                        Description="The category is technology"
                    }
                });
            var categoryBusiness = new CategoryBusiness(_mockCategoryRepository.Object);
            var response = (await categoryBusiness.GetAllCategoriesAsync()).ToList();
            Assert.NotNull(response);
            Assert.Equal("Category 1", response.First().Name);
            Assert.Equal(2, response.Count);
        }

        [Fact]
        public async Task GetCategoryByIdAsync_WhenIdCategoryExist_ShouldReturnCategoryById()
        {
            _mockCategoryRepository.Setup(c => c.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(
            new Category
            {
                Name = "Electrodomesticos",
                Description = "Seccion de tecnologia",
                CreationDate = new DateTime(2020, 01, 02),
                ModificationDate = new DateTime(2021, 02, 04)
            });
            var categoryBusiness = new CategoryBusiness(_mockCategoryRepository.Object);
            var category = await categoryBusiness.GetCategoryByIdAsync(Guid.Empty);
            Assert.NotNull(category);
            Assert.Equal("Electrodomesticos", category.Name);
        }

        [Fact]
        public async Task UpdateCategoryAsync_WhenCategoryIsNull_ShouldThrowCategoryInvalidException()
        {
            var categoryBusiness = new CategoryBusiness(_mockCategoryRepository.Object);
            var exception = await Assert.ThrowsAsync<CategoryInvalidException>(() => categoryBusiness.UpdateCategoryAsync(null));
            Assert.Contains("The category is null", exception.Message);
            _mockCategoryRepository.Verify(c => c.UpdateAsync(It.IsAny<Category>()), Times.Never);
        }

        [Fact]
        public async Task UpdateCategoryAsync_WhenCategoryIsEmpty_ShouldThrowCategoryInvalidException()
        {
            var categoryBusiness = new CategoryBusiness(_mockCategoryRepository.Object);
            var exception = await Assert.ThrowsAsync<CategoryInvalidException>(() => categoryBusiness.UpdateCategoryAsync(new Category { }));
            Assert.Contains("The name category is empty", exception.Message);
            _mockCategoryRepository.Verify(c => c.UpdateAsync(It.IsAny<Category>()), Times.Never);
        }

        [Fact]
        public async Task UpdateCategoryAsync_WhenCategoryIsOk_ShouldUpdateCategory()
        {
            _mockCategoryRepository.Setup(c => c.UpdateAsync(It.IsAny<Category>()));
            var categoryBusiness = new CategoryBusiness(_mockCategoryRepository.Object);
            var category = await categoryBusiness.UpdateCategoryAsync(new Category
            {
                Name = "Lacteos",
                Description = "Productos refrigerados",
                CreationDate = new DateTime(2023, 03, 09),
                ModificationDate = new DateTime(2021, 03, 4)
            });
            Assert.True(category);
            _mockCategoryRepository.Verify(c => c.UpdateAsync(It.IsAny<Category>()), Times.Once);
        }

        [Fact]
        public async Task UpdateCategoryAsync_WhenCategoryExists_ShouldThrowCategoryInvalidException()
        {
            Category category = null;
            _mockCategoryRepository.Setup(m => m.CreateAsync(It.IsAny<Category>()))
                .Callback<Category>(c => category = c)
                .ReturnsAsync(true);
            var categoryBusiness = new CategoryBusiness(_mockCategoryRepository.Object);
            var exception = await Assert.ThrowsAsync<CategoryInvalidException>(() => categoryBusiness.UpdateCategoryAsync(new Category { Name = "Aseo" }));
            Assert.Contains("The category already exists", exception.Message);
            Assert.Contains("Aseo", category.Name);
            _mockCategoryRepository.Verify(c => c.UpdateAsync(It.IsAny<Category>()), Times.Never);
        }

        [Fact]
        public async Task GetCategoryByNameAsync_WhenCategoryIsNull_ShouldReturnNull()
        {
            var categoryBusiness = new CategoryBusiness(_mockCategoryRepository.Object);
            var result = await categoryBusiness.GetCategoryByNameAsync(string.Empty);
            Assert.Null(result);
        }

        [Fact]
        public async Task GetCategoryByNameAsync_WhenCategoryIsOk_ShoulGetCategoryByName()
        {
            _mockCategoryRepository.Setup(c => c.GetCategoryByNameAsync(It.IsAny<string>())).ReturnsAsync(
            new Category
            {
                Name = "Electrodomesticos"
            });
            var categoryBusiness = new CategoryBusiness(_mockCategoryRepository.Object);
            var category = await categoryBusiness.GetCategoryByNameAsync("Electrodomesticos");
            Assert.Equal("Electrodomesticos", category.Name);
        }
    }
}