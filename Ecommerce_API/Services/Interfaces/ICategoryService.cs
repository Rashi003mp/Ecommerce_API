using Ecommerce_API.Common;
using Ecommerce_API.DTOs.CategoryDTO;

namespace Ecommerce_API.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<ApiResponse<IEnumerable<CategoryDTO>>> GetAllAsync();
        Task<ApiResponse<CategoryDTO?>> GetByIdAsync(int id);

        Task<ApiResponse<CategoryDTO>> AddAsync(CategoryDTO dto);

        Task<ApiResponse<CategoryDTO?>> UpdateAsync(int id, CategoryDTO dto);

        Task<ApiResponse<bool>> DeleteAsync(int id);
    }
}
