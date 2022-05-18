using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TiendaServices.Entities;

namespace TiendaServices.Business.Intarfaces
{
    public interface ICategoryBusiness
    {
        Task<bool> CreateCategoryAsync(Category category);

        Task<bool> UpdateCategoryAsync(Category category);

        Task<bool> DeleteCategoryAsync(Guid id);

        Task<IEnumerable<Category>> GetAllCategoriesAsync();

        Task<Category> GetCategoryByIdAsync(Guid id);

        Task<Category> GetCategoryByNameAsync(string name);
    }
}