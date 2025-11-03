using Ecommerce_API.Common;
using Ecommerce_API.Models;

namespace Ecommerce_API.Services.Interfaces
{
    public interface IUserService
    {
        Task<ApiResponse<IEnumerable<User>>> GetAllUsersAsync();
        Task<ApiResponse<User>> GetUserByIdAsync(int id);

        Task<ApiResponse<string>> BlockUnblockUserAsync(int id);
        Task<ApiResponse<string>> SoftDeleteUserAsync(int id);
    }
}
