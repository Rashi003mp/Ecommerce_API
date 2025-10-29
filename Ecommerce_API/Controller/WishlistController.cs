using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Ecommerce_API.Services;
using Ecommerce_API.Services.Implementation;
using Ecommerce_API.Services.Interfaces;

namespace Ecommerce_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService _wishListService;

        public WishlistController(IWishlistService wishListService)
        {
            _wishListService = wishListService;
        }

        // Securely extract user ID from JWT claim
        private int GetUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
            {
                throw new ArgumentException("User ID not found in token");
            }
            return int.Parse(claim.Value);
        }

        [Authorize(Policy = "Customer")]
        [HttpGet]
        public async Task<IActionResult> GetWishlist()
        {
            int userId = GetUserId();
            var response = await _wishListService.GetWishlistAsync(userId);
            return StatusCode(response.StatusCode, response);
        }

        [Authorize(Policy = "Customer")]
        [HttpPost("{productId}")]
        public async Task<IActionResult> ToggleWishlist(int productId)
        {
            int userId = GetUserId();
            var response = await _wishListService.ToggleWishlistasync(userId, productId);
            return StatusCode(response.StatusCode, response);
        }
    }
}
