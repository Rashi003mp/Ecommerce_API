using Ecommerce_API.DTOs.AuthDTO;

namespace Ecommerce_API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<AuthResponseDto> LoginAsync(LoginDTO loginDto);
    }
}
