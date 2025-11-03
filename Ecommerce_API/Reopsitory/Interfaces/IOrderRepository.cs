using Ecommerce_API.Entities;

namespace Ecommerce_API.Reopsitory.Interfaces
{
    public interface IOrderRepository :IGenericRepository<Order>
    {
        // ---- User-related operations ----
        Task<List<CartItem>> GetCartItemsByUserAsync(int userId);
        Task<Product?> GetProductByIdAsync(int productId);
        Task<Order> CreateOrderAsync(Order order);
        Task<Order?> GetOrderByIdAsync(int orderId);
        Task<List<Order>> GetOrdersByUserIdAsync(int userId);
        Task<bool> DeleteCartItemsAsync(IEnumerable<CartItem> cartItems);

        Task UpdateOrderAsync(Order order);

        // ---- Admin operations ----
        Task<List<Order>> GetAllOrdersAsync(int pageNumber, int limit);
        Task<int> GetOrdersCountAsync();
        Task<List<Order>> SearchOrdersAsync(string username);
        Task<List<Order>> GetOrdersByStatusAsync(OrderStatus status);
        Task<List<Order>> SortOrdersByDateAsync(bool ascending);

        // ---- Payment-related ----
        //Task<Order?> GetByRazorpayOrderIdAsync(string razorpayOrderId);
    }

}
