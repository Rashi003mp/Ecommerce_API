using Ecommerce_API.Common;
using Ecommerce_API.DTOs.ProductDTO;

namespace Ecommerce_API.Services.Interfaces
{
    public interface IProductService
    {
        Task<ApiResponse<ProductDTO?>> GetProductByIdAsync(int id);



        //    Task<IEnumerable<ProductDTO>> GetProductsByCategoryAsync(int categoryId);
        //    Task<IEnumerable<ProductDTO>> GetAllProductsAsync();
        Task<ApiResponse<IEnumerable<ProductDTO>>> GetFilteredProducts(
        string? name = null,
        int? categoryId = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        bool? inStock = null,
        int page = 1,
        int pageSize = 20,
        string? sortBy = null,
        bool descending = false
    );
        Task<ApiResponse<ProductDTO>> AddProductAsync(CreateProductDTO model);

        Task<ApiResponse<string>> UpdateProductAsync(int id, UpdateProductDTO model);



    }
}
