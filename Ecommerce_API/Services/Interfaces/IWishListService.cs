using Ecommerce_API.Common;

namespace Ecommerce_API.Services.Interfaces
{
    public interface IWishlistService
    {
        Task<ApiResponse<object>> GetWishlistAsync(int userId);
        Task<ApiResponse<string>> ToggleWishlistasync(int userId, int ProductId);

    }
}
