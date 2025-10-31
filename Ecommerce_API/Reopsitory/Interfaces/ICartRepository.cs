using Ecommerce_API.Entities;

namespace Ecommerce_API.Reopsitory.Interfaces
{
    public interface ICartRepository :IGenericRepository<Cart>
    {
        Task<Cart?> GetCartWithItemsByUserIdAsync(int userId);
        Task<CartItem?> GetCartItemByIdAsync(int cartItemId, int userId);
        //void Update(CartItem cartItem);
        Task UpdateCartItemAsync(CartItem cartItem);
        Task ClearCartForUserAsync(int userId);

    }
}
