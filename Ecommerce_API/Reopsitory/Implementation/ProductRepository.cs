using Ecommerce_API.Data;
using Ecommerce_API.Entities;
using Ecommerce_API.Reopsitory.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_API.Reopsitory.Implementation
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Where(p => p.CategoryId == categoryId && p.IsActive && !p.IsDeleted)
                .ToListAsync();
        }

        public async Task<Product?> GetProductWithDetailsAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Product>> GetFilteredProductsAsync(
    string? name,
    int? categoryId,
    decimal? minPrice,
    decimal? maxPrice,
    bool? inStock,
    int page,
    int pageSize,
    string? sortBy,
    bool descending)
        {
            var query = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Images)
                .AsQueryable();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(p => p.Name.Contains(name));

            if (categoryId.HasValue)
                query = query.Where(p => p.CategoryId == categoryId.Value);

            if (minPrice.HasValue)
                query = query.Where(p => p.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(p => p.Price <= maxPrice.Value);

            if (inStock.HasValue)
                query = query.Where(p => p.InStock == inStock.Value);

            // Sorting
            query = sortBy?.ToLower() switch
            {
                "price" => descending ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price),
                "name" => descending ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name),
                _ => query.OrderBy(p => p.Id)
            };

            // Pagination
            query = query.Skip((page - 1) * pageSize).Take(pageSize);

            return await query.ToListAsync();
        }


    }
}
