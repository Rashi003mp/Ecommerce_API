using Ecommerce_API.DTOs.CategoryDTO;
using Ecommerce_API.Entities;

namespace Ecommerce_API.Reopsitory.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task AddAsync(CategoryDTO newCategory);
        Task<Category?> GetByNameAsync(string name);

    }
}
