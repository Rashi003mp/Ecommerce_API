using AutoMapper;
using Ecommerce_API.DTOs.AddressDTO;
using Ecommerce_API.Entities;
using Ecommerce_API.Reopsitory.Interfaces;
using Ecommerce_API.Services.Interfaces;

namespace Ecommerce_API.Services.Implementation
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;
        public AddressService(IAddressRepository addressRepository , IMapper mapper)
        {
            _addressRepository = addressRepository;
            _mapper = mapper;
        }

        // Get all addresses for a user
        public async Task<IEnumerable<AddressDto>> GetUserAddressesAsync(int userId)
        {
            var addresses = await _addressRepository.GetUserAddressesAsync(userId);
            return _mapper.Map<IEnumerable<AddressDto>>(addresses);
        }

        // Get specific address
        public async Task<AddressDto?> GetAddressByIdAsync(int id, int userId)
        {
            var address = await _addressRepository.GetAddressByIdAsync(id, userId);
            return address == null ? null : _mapper.Map<AddressDto>(address);
        }

        // Create new address
        public async Task<AddressDto> CreateAddressAsync(AddressDto dto, int userId)
        {
            var address = _mapper.Map<Address>(dto);
            address.UserId = userId;

            await _addressRepository.AddAsync(address);

            return _mapper.Map<AddressDto>(address);
        }

        // Update address
        public async Task<AddressDto?> UpdateAddressAsync(int id, AddressDto dto, int userId)
        {
            var existing = await _addressRepository.GetAddressByIdAsync(id, userId);
            if (existing == null) return null;

            // Map updated fields onto existing entity
            _mapper.Map(dto, existing);

            await _addressRepository.UpdateAsync(existing);
            return _mapper.Map<AddressDto>(existing);
        }

        // Delete address
        public async Task<bool> DeleteAddressAsync(int id, int userId)
        {
            var address = await _addressRepository.GetAddressByIdAsync(id, userId);
            if (address == null) return false;

            await _addressRepository.DeleteAsyncByEntity(address);
            return true;
        }
    }
}
