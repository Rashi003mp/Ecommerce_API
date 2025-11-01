using AutoMapper;
using Ecommerce_API.Common;
using Ecommerce_API.DTOs.OrderDTO;
using Ecommerce_API.Entities;
using Ecommerce_API.Reopsitory.Interfaces;
using Ecommerce_API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_API.Services.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public OrderService(
            IOrderRepository orderRepository,
            ICartRepository cartRepository,
            IAddressRepository addressRepository,
            IProductRepository productRepository,
            IMapper mapper)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _addressRepository = addressRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        // ✅ Create Order (Cart or BuyNow)
        public async Task<ApiResponse<object>> CreateOrderAsync(int userId, CreateOrderDTO dto, BuyNowDTO buyNowDto = null)
        {
            // 🔹 Step 1: Ensure Address (create if new)
            Address address;
            if (dto.AddressId == 0)
            {
                address = new Address
                {
                    UserId = userId,
                    AddressLine1 = dto.NewAddress?.AddressLine1,
                    AddressLine2 = dto.NewAddress?.AddressLine2,
                    City = dto.NewAddress?.City,
                    State = dto.NewAddress?.State,
                    PostalCode = dto.NewAddress?.PostalCode,
                    Country = dto.NewAddress?.Country
                };

                await _addressRepository.AddAsync(address);
                await _addressRepository.SaveChangesAsync();
            }
            else
            {
                address = await _addressRepository.GetAddressByIdAsync(dto.AddressId, userId);
                if (address == null || address.UserId != userId)
                    return new ApiResponse<object>(404, "Invalid or unauthorized address");
            }

            // 🔹 Step 2: Build order items (from BuyNow or Cart)
            List<OrderItem> orderItems = new();

            if (buyNowDto != null)
            {
                var product = await _productRepository.GetByIdAsync(buyNowDto.ProductId);
                if (product == null || product.IsDeleted || !product.IsActive)
                    return new ApiResponse<object>(404, "Product not available");

                if (product.CurrentStock < buyNowDto.Quantity)
                    return new ApiResponse<object>(400, "Insufficient stock");

                orderItems.Add(new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = buyNowDto.Quantity,
                    Price = product.Price
                });

                product.CurrentStock -= buyNowDto.Quantity;
                await _productRepository.UpdateAsync(product);
            }
            else
            {
                var cart = await _cartRepository.GetCartWithItemsByUserIdAsync(userId);
                if (cart == null || cart.Items.Count == 0)
                    return new ApiResponse<object>(404, "Cart is empty");

                foreach (var item in cart.Items.Where(i => !i.IsDeleted))
                {
                    if (item.Product.CurrentStock < item.Quantity)
                        return new ApiResponse<object>(400, $"Insufficient stock for {item.Product.Name}");

                    orderItems.Add(new OrderItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = item.Product.Price
                    });

                    item.Product.CurrentStock -= item.Quantity;
                    await _productRepository.UpdateAsync(item.Product);
                }

                // Clear the cart after order creation
                await _cartRepository.ClearCartForUserAsync(cart.UserId);
            }

            // 🔹 Step 3: Create Order
            var order = new Order
            {
                UserId = userId,
                AddressId = address.Id,
                PaymentMethod = dto.PaymentMethod,
                OrderStatus = OrderStatus.Pending,
                TotalAmount = orderItems.Sum(i => i.Quantity * i.Price),
                Items = orderItems
            };

            await _orderRepository.CreateOrderAsync(order);

            // 🔹 Step 4: Return response
            var orderResponse = _mapper.Map<OrderResponseDTO>(order);
            return new ApiResponse<object>(200, "Order created successfully", orderResponse);
        }

        // ✅ Update Order Status (Admin)
        public async Task<ApiResponse<OrderResponseDTO>> UpdateOrderStatus(int orderId, OrderStatus newStatus)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
                return new ApiResponse<OrderResponseDTO>(404, "Order not found");

            order.OrderStatus = newStatus;
            order.ModifiedOn = DateTime.UtcNow;

            await _orderRepository.UpdateAsync(order);

            var mapped = _mapper.Map<OrderResponseDTO>(order);
            return new ApiResponse<OrderResponseDTO>(200, "Order status updated", mapped);
        }

        // ✅ Get Orders by User
        public async Task<ApiResponse<IEnumerable<OrderResponseDTO>>> GetOrdersByUserIdAsync(int userId)
        {
            var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);
            var mapped = _mapper.Map<IEnumerable<OrderResponseDTO>>(orders);
            return new ApiResponse<IEnumerable<OrderResponseDTO>>(200, "Orders fetched successfully", mapped);
        }

        // ✅ Cancel Order
        public async Task<ApiResponse<bool>> CancelOrderAsync(int userId, int orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null || order.UserId != userId)
                return new ApiResponse<bool>(404, "Order not found");

            if (order.OrderStatus != OrderStatus.Pending)
                return new ApiResponse<bool>(400, "Only pending orders can be cancelled");

            order.OrderStatus = OrderStatus.Cancelled;
            await _orderRepository.UpdateAsync(order);

            return new ApiResponse<bool>(200, "Order cancelled successfully", true);
        }

        // ✅ Admin — Get All Orders (Paged)
        //public async Task<ApiResponse<PagedResult<OrderResponseDTO>>> GetAllOrdersAsync(int pageNumber, int limit)
        //{
        //    var paged = await _orderRepository.GetAllOrdersAsync(pageNumber, limit);
        //    var mapped = new PagedResult<OrderResponseDTO>
        //    {
        //        Items = _mapper.Map<IEnumerable<OrderResponseDTO>>(paged.Items),
        //        TotalCount = paged.TotalCount,
        //        TotalPages = paged.TotalPages
        //    };
        //    return new ApiResponse<PagedResult<OrderResponseDTO>>(200, "Orders fetched", mapped);
        //}
        public async Task<ApiResponse<PagedResult<OrderResponseDTO>>> GetAllOrdersAsync(int pageNumber, int limit)
        {
            var totalOrders = await _orderRepository.GetOrdersCountAsync();
            var orders = await _orderRepository.GetAllOrdersAsync(pageNumber, limit);

            var totalPages = (int)Math.Ceiling((double)totalOrders / limit);
            var result = new PagedResult<OrderResponseDTO>
            {
                Items = _mapper.Map<List<OrderResponseDTO>>(orders),
                CurrentPage = pageNumber,
                PageSize = limit,
                TotalItems = totalOrders,
                TotalPages = totalPages
            };
            return new ApiResponse<PagedResult<OrderResponseDTO>>(200, "Successfully fetched orders", result);
        }


        // ✅ Get Order by ID
        public async Task<ApiResponse<OrderResponseDTO>> GetOrderbyIdAsync(int orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
                return new ApiResponse<OrderResponseDTO>(404, "Order not found");

            var mapped = _mapper.Map<OrderResponseDTO>(order);
            return new ApiResponse<OrderResponseDTO>(200, "Order fetched successfully", mapped);
        }

        // ✅ Search Orders by Username (Admin)
        public async Task<ApiResponse<IEnumerable<OrderResponseDTO>>> SearchOrdersAsync(string username)
        {
            var orders = await _orderRepository.SearchOrdersAsync(username);
            var mapped = _mapper.Map<IEnumerable<OrderResponseDTO>>(orders);
            return new ApiResponse<IEnumerable<OrderResponseDTO>>(200, "Orders fetched successfully", mapped);
        }

        // ✅ Filter Orders by Status (Admin)
        public async Task<ApiResponse<IEnumerable<OrderResponseDTO>>> GetOrdersByStatus(OrderStatus status)
        {
            var orders = await _orderRepository.GetOrdersByStatusAsync(status);
            var mapped = _mapper.Map<IEnumerable<OrderResponseDTO>>(orders);
            return new ApiResponse<IEnumerable<OrderResponseDTO>>(200, "Orders fetched successfully", mapped);
        }

        // ✅ Sort Orders by Date (Admin)
        public async Task<ApiResponse<IEnumerable<OrderResponseDTO>>> SortOrdersByDateAsync(bool ascending)
        {
            var orders = await _orderRepository.SortOrdersByDateAsync(ascending);
            var mapped = _mapper.Map<IEnumerable<OrderResponseDTO>>(orders);
            return new ApiResponse<IEnumerable<OrderResponseDTO>>(200, "Orders sorted successfully", mapped);
        }

        /*
        // 🔒 Razorpay Payment Verification (Enable after payment setup)
        public async Task<ApiResponse<object>> VerifyRazorpayPaymentAsync(PaymentVerifyDto dto)
        {
            var verified = RazorpayUtils.VerifyPaymentSignature(dto);
            if (!verified)
                return new ApiResponse<object>(400, "Payment verification failed");

            // Update order status to "Paid"
            var order = await _orderRepository.GetOrderByIdAsync(dto.OrderId);
            if (order == null) return new ApiResponse<object>(404, "Order not found");

            order.OrderStatus = OrderStatus.Paid;
            await _orderRepository.UpdateAsync(order);

            return new ApiResponse<object>(200, "Payment verified successfully", order);
        }
        */
    }
}
