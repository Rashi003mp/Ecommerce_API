using Ecommerce_API.Data;
using Ecommerce_API.Entities;
using Ecommerce_API.Reopsitory.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_API.Reopsitory.Implementation
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly AppDbContext _context;
        private readonly GenericRepository<Order> _OrderRepo;


        public OrderRepository(AppDbContext context) : base(context)
        {
            _context = context;
            _OrderRepo = new GenericRepository<Order>(context);
        }

        // 🛒 USER: Get cart items for checkout
        public async Task<List<CartItem>> GetCartItemsByUserAsync(int userId)
        {
            return await _context.CartItems
                .Include(c => c.Product)
                .Where(c => c.Cart.UserId == userId && !c.IsDeleted)
                .ToListAsync();
        }

        // 🛍️ USER: Get a single product (for Buy Now)
        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.Id == productId && !p.IsDeleted && p.IsActive);
        }

        // 💾 USER: Create a new order
        public async Task<Order> CreateOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return order;
        }

        // 🧾 USER: Get single order details (by ID)
        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.Items)
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        // 📦 USER: Get all orders for a user
        public async Task<List<Order>> GetOrdersByUserIdAsync(int userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.Items)
                    .ThenInclude(oi => oi.Product)
                .OrderByDescending(o => o.CreatedOn)
                .ToListAsync();
        }

        // 🗑️ USER: Delete cart items after placing order
        public async Task<bool> DeleteCartItemsAsync(IEnumerable<CartItem> cartItems)
        {
            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
            return true;
        }

        // 💳 PAYMENT: Get order by Razorpay Order ID
        //public async Task<Order?> GetByRazorpayOrderIdAsync(string razorpayOrderId)
        //{
        //    return await _context.Orders
        //        .FirstOrDefaultAsync(o => o.RazorpayOrderId == razorpayOrderId);
        //}

        // 💳 PAYMENT: Update order status (e.g., after payment success)
        public async Task UpdateOrderAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        // 👩‍💼 ADMIN: Get all orders (paginated)
        public async Task<List<Order>> GetAllOrdersAsync(int pageNumber, int limit)
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Items)
                .OrderByDescending(o => o.CreatedOn)
                .Skip((pageNumber - 1) * limit)
                .Take(limit)
                .ToListAsync();
        }

        // 👩‍💼 ADMIN: Get total order count
        public async Task<int> GetOrdersCountAsync()
        {
            return await _context.Orders.CountAsync();
        }

        // 👩‍💼 ADMIN: Search orders by username
        public async Task<List<Order>> SearchOrdersAsync(string username)
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Items)
                .Where(o => o.User.Name.Contains(username))
                .OrderByDescending(o => o.CreatedOn)
                .ToListAsync();
        }

        // 👩‍💼 ADMIN: Filter orders by status (Pending, Shipped, etc.)
        public async Task<List<Order>> GetOrdersByStatusAsync(OrderStatus status)
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Items)
                .Where(o => o.OrderStatus == status)
                .OrderByDescending(o => o.CreatedOn)
                .ToListAsync();
        }

        // 👩‍💼 ADMIN: Sort orders by date (ascending/descending)
        public async Task<List<Order>> SortOrdersByDateAsync(bool ascending)
        {
            var query = _context.Orders
                .Include(o => o.User)
                .Include(o => o.Items)
                .AsQueryable();

            query = ascending
                ? query.OrderBy(o => o.CreatedOn)
                : query.OrderByDescending(o => o.CreatedOn);

            return await query.ToListAsync();
        }
    }
}
