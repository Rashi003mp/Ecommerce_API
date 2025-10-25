using Ecommerce_API.Entities;

namespace Ecommerce_API.Reopsitory.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId);
        Task<Product?> GetProductWithDetailsAsync(int id);
    }
}
