using Ecommerce_API.DTOs.AddressDTO;
using Ecommerce_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommerce_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] 
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdClaim, out var userId) ? userId : 0;
        }

        // api/addresses
        [HttpGet]
        public async Task<IActionResult> GetUserAddresses()
        {
            var userId = GetUserId();
            if (userId == 0) return Unauthorized();

            var addresses = await _addressService.GetUserAddressesAsync(userId);
            return Ok(addresses);
        }

        ///api/addresses/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAddressById(int id)
        {
            var userId = GetUserId();
            if (userId == 0) return Unauthorized();

            var address = await _addressService.GetAddressByIdAsync(id, userId);
            if (address == null)
                return NotFound(new { message = "Address not found" });

            return Ok(address);
        }

        // /api/addresses
        [HttpPost]
        public async Task<IActionResult> CreateAddress([FromBody] AddressDto dto)
        {
            var userId = GetUserId();
            if (userId == 0) return Unauthorized();

            var created = await _addressService.CreateAddressAsync(dto, userId);
            return CreatedAtAction(nameof(GetAddressById), new { id = created.Id }, created);
        }

        // /api/addresses/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAddress(int id, [FromBody] AddressDto dto)
        {
            var userId = GetUserId();
            if (userId == 0) return Unauthorized();

            var updated = await _addressService.UpdateAddressAsync(id, dto, userId);
            if (updated == null)
                return NotFound(new { message = "Address not found" });

            return Ok(updated);
        }

        //  /api/addresses/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            var userId = GetUserId();
            if (userId == 0) return Unauthorized();

            var deleted = await _addressService.DeleteAddressAsync(id, userId);
            if (!deleted)
                return NotFound(new { message = "Address not found" });

            return Ok(new { message = "Address deleted successfully" });
        }
    }
}
