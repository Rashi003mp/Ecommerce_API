using Ecommerce_API.DTOs.AddressDTO;
using Ecommerce_API.Entities;
using Ecommerce_API.Reopsitory.Interfaces;
using Ecommerce_API.Services.Interfaces;

namespace Ecommerce_API.Services.Implementation
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;

        public AddressService(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        // Get all addresses for a user
        public async Task<IEnumerable<AddressDto>> GetUserAddressesAsync(int userId)
        {
            var addresses = await _addressRepository.GetUserAddressesAsync(userId);

            // Manual mapping to DTOs
            return addresses.Select(a => new AddressDto
            {
                Id = a.Id,
                FullName = a.FullName,
                PhoneNumber = a.PhoneNumber,
                AddressLine1 = a.AddressLine1,
                AddressLine2 = a.AddressLine2,
                City = a.City,
                State = a.State,
                Country = a.Country,
                PostalCode = a.PostalCode,
                IsDefault = a.IsDefault
            });
        }

        // Get specific address
        public async Task<AddressDto?> GetAddressByIdAsync(int id, int userId)
        {
            var address = await _addressRepository.GetAddressByIdAsync(id, userId);
            if (address == null) return null;

            return new AddressDto
            {
                Id = address.Id,
                FullName = address.FullName,
                PhoneNumber = address.PhoneNumber,
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2,
                City = address.City,
                State = address.State,
                Country = address.Country,
                PostalCode = address.PostalCode,
                IsDefault = address.IsDefault
            };
        }

        // Create new address
        public async Task<AddressDto> CreateAddressAsync(AddressDto dto, int userId)
        {
            var address = new Address
            {
                UserId = userId,
                FullName = dto.FullName,
                PhoneNumber = dto.PhoneNumber,
                AddressLine1 = dto.AddressLine1,
                AddressLine2 = dto.AddressLine2,
                City = dto.City,
                State = dto.State,
                Country = dto.Country,
                PostalCode = dto.PostalCode,
                IsDefault = dto.IsDefault
            };

            await _addressRepository.AddAsync(address);

            dto.Id = address.Id;
            return dto;
        }

        // Update address
        public async Task<AddressDto?> UpdateAddressAsync(int id, AddressDto dto, int userId)
        {
            var existing = await _addressRepository.GetAddressByIdAsync(id, userId);
            if (existing == null) return null;

            existing.FullName = dto.FullName;
            existing.PhoneNumber = dto.PhoneNumber;
            existing.AddressLine1 = dto.AddressLine1;
            existing.AddressLine2 = dto.AddressLine2;
            existing.City = dto.City;
            existing.State = dto.State;
            existing.Country = dto.Country;
            existing.PostalCode = dto.PostalCode;
            existing.IsDefault = dto.IsDefault;

            await _addressRepository.UpdateAsync(existing);

            return dto;
        }

        // Delete address
        public async Task<bool> DeleteAddressAsync(int id, int userId)
        {
            var address = await _addressRepository.GetAddressByIdAsync(id, userId);
            Console.WriteLine(address);
            if (address == null) return false;

            await _addressRepository.DeleteAsyncByEntity(address);
            return true;
        }
    }
}
