using Ecommerce_API.Data;
using Ecommerce_API.Entities;
using Ecommerce_API.Models;
using Ecommerce_API.Reopsitory.Implementation;
using Ecommerce_API.Reopsitory.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Ecommerce_API.Repositories.Implementation
{
    public class CartRepository : GenericRepository<Cart>, ICartRepository
    {
        private readonly AppDbContext _context;
        private readonly GenericRepository<CartItem> _cartItemRepo;

        public CartRepository(AppDbContext context) : base(context)
        {
            _context = context;
            _cartItemRepo = new GenericRepository<CartItem>(context);
        }

        public async Task<Cart?> GetCartWithItemsByUserIdAsync(int userId)
        {
            var cart = await _context.Carts
         .Where(c => c.UserId == userId && !c.IsDeleted)
         .Include(c => c.Items)
             .ThenInclude(i => i.Product)
         .FirstOrDefaultAsync();

            if (cart != null)
            {
                cart.Items = cart.Items
                    .Where(i => i.Product != null && !i.Product.IsDeleted && i.Product.IsActive)
                    .ToList();
            }

            return cart;
        }

        public async Task<CartItem?> GetCartItemByIdAsync(int cartItemId, int userId)
        {
            return await _context.CartItems
                .Include(ci => ci.Cart)
                .FirstOrDefaultAsync(ci => ci.Id == cartItemId
                                           && ci.Cart.UserId == userId
                                           && !ci.Cart.IsDeleted);
        }

        public async Task UpdateCartItemAsync(CartItem cartItem)
        {
            await _cartItemRepo.UpdateAsync(cartItem);
        }

        public async Task ClearCartForUserAsync(int userId)
        {
            var cart = await GetCartWithItemsByUserIdAsync(userId);
            if (cart != null && cart.Items.Any())
            {
                _context.CartItems.RemoveRange(cart.Items);
                await _context.SaveChangesAsync();
            }
        }
    }
}
