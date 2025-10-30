using Ecommerce_API.Data;
using Ecommerce_API.DTOs.CategoryDTO;
using Ecommerce_API.Entities;
using Ecommerce_API.Reopsitory.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_API.Reopsitory.Implementation
{
    
   public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public readonly AppDbContext _context;
        public CategoryRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public Task AddAsync(CategoryDTO newCategory)
        {
            throw new NotImplementedException();
        }

        public async Task<Category?> GetByNameAsync(string name)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower());
        }
    }

}
