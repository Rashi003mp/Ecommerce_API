using Ecommerce_API.DTOs.AddressDTO;


namespace Ecommerce_API.Services.Interfaces
{
    public interface IAddressService
    {
        Task<IEnumerable<AddressDto>> GetUserAddressesAsync(int userId);
        Task<AddressDto?> GetAddressByIdAsync(int id, int userId);
        Task<AddressDto> CreateAddressAsync(AddressDto dto, int userId);
        Task<AddressDto?> UpdateAddressAsync(int id, AddressDto dto, int userId);
        Task<bool> DeleteAddressAsync(int id, int userId);
    }
}
