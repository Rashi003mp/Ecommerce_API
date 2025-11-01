using Ecommerce_API.Common;
using Ecommerce_API.DTOs.OrderDTO;
using Ecommerce_API.Entities;

namespace Ecommerce_API.Services.Interfaces
{
    public interface IOrderService
    {
        Task<ApiResponse<object>> CreateOrderAsync(int userId, CreateOrderDTO dto, BuyNowDTO buyNowDto = null);

        Task<ApiResponse<OrderResponseDTO>> UpdateOrderStatus(int orderId, OrderStatus newStatus);
        Task<ApiResponse<IEnumerable<OrderResponseDTO>>> GetOrdersByUserIdAsync(int userId);
        Task<ApiResponse<bool>> CancelOrderAsync(int userId, int orderId);

        Task<ApiResponse<PagedResult<OrderResponseDTO>>> GetAllOrdersAsync(int pagenumber, int limit);
        Task<ApiResponse<OrderResponseDTO>> GetOrderbyIdAsync(int orderId);
        Task<ApiResponse<IEnumerable<OrderResponseDTO>>> SearchOrdersAsync(string username);
        Task<ApiResponse<IEnumerable<OrderResponseDTO>>> GetOrdersByStatus(OrderStatus status);
        Task<ApiResponse<IEnumerable<OrderResponseDTO>>> SortOrdersByDateAsync(bool ascending);
        //Task<ApiResponse<object>> VerifyRazorpayPaymentAsync(PaymentVerifyDto dto);
    }
}
