using Ecommerce_API.Common;
using Ecommerce_API.Models;
using Ecommerce_API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var response = await _userService.GetAllUsersAsync();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var response = await _userService.GetUserByIdAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("block-unblock/{id:int}")]
        public async Task<IActionResult> BlockUnblockUser(int id)
        {
            var response = await _userService.BlockUnblockUserAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("soft-delete/{id:int}")]
        public async Task<IActionResult> SoftDeleteUser(int id)
        {
            var response = await _userService.SoftDeleteUserAsync(id);
            return StatusCode(response.StatusCode, response);
        }
    }
}
