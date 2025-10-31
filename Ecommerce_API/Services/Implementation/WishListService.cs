using Ecommerce_API.Common;
using Ecommerce_API.Data;
using Ecommerce_API.Entities;
using Ecommerce_API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_API.Services.Implementation
{
    public class WishListService : IWishlistService
    {
        private readonly AppDbContext _Context;

        public WishListService(AppDbContext context)
        {
            _Context = context;
        }

        public async Task<ApiResponse<object>> GetWishlistAsync(int userId)
        {
            var items = await _Context.Wishlists
                .Where(x => x.UserId == userId && x.Product.IsActive)
                .Include(x => x.Product)
                    .ThenInclude(p => p.Images)
                .AsNoTracking()
                .ToListAsync();

            var result = items.Select(i => new
            {
                i.ProductId,
                i.Product.Name,
                i.Product.Price,
                MainImageUrl = i.Product.Images.FirstOrDefault(img => img.IsMain)?.ImageUrl
            });

            return new ApiResponse<object>(200, "Wishlist fetched successfully", result);
        }


        public async Task<ApiResponse<string>> ToggleWishlistasync(int userId, int productId)
        {
            var existing = await _Context.Wishlists
                .FirstOrDefaultAsync(w => w.UserId == userId && w.ProductId == productId && !w.Product.IsActive);

            if (existing != null)
            {
                _Context.Wishlists.Remove(existing);
                await _Context.SaveChangesAsync();
                return new ApiResponse<string>(200, "Product removed from wishlist");
            }

            var user = await _Context.Users.FindAsync(userId);
            if (user == null)
            {
                return new ApiResponse<string>(404, "User not found");
            }

            var product = await _Context.Products.FindAsync(productId);
            if (product == null || !product.IsActive)
            {
                return new ApiResponse<string>(404, "Product not found or inactive");
            }

            var wishlist = new Wishlist
            {
                UserId = userId,
                ProductId = productId
            };

            _Context.Wishlists.Add(wishlist);

            try
            {
                await _Context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return new ApiResponse<string>(500, $"Error saving wishlist: {ex.InnerException?.Message}");
            }

            return new ApiResponse<string>(200, "Product added to wishlist");
        }
    }
}
