using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TiendaServices.Business.Intarfaces;
using TiendaServices.Common.Exceptions;
using TiendaServices.DataAccess.Interfaces;
using TiendaServices.Entities;

namespace TiendaServices.Business.Implementations
{
    public class CategoryBusiness : ICategoryBusiness
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryBusiness(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<bool> CreateCategoryAsync(Category category)
        {
            await Validate(category);
            var isCreated = await _categoryRepository.CreateAsync(category);
            return isCreated;
        }

        public async Task<bool> DeleteCategoryAsync(Guid id)
        {
            var isDeleted = await _categoryRepository.DeleteAsync(id);
            return isDeleted;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return categories;
        }

        public async Task<Category> GetCategoryByIdAsync(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            return category;
        }

        public async Task<bool> UpdateCategoryAsync(Category category)
        {
            await Validate(category);
            var isUpdated = await _categoryRepository.UpdateAsync(category);
            return isUpdated;
        }

        public async Task<Category> GetCategoryByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }
            var category = await _categoryRepository.GetCategoryByNameAsync(name);
            return category;
        }

        private async Task Validate(Category category)
        {
            if (category == null)
            {
                throw new CategoryInvalidException("The category is null");
            }
            if (string.IsNullOrEmpty(category.Name))
            {
                throw new CategoryInvalidException("The name category is empty");
            }
            if (await _categoryRepository.GetCategoryByNameAsync(category.Name) != null)
            {
                throw new CategoryInvalidException("The category already exists");
            }
        }
    }
}