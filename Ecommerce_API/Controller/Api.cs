using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_API.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class Api : ControllerBase
    {
        private readonly List<string> users = new()
        {
            "Alice",
            "Bob",
            "Charlie",
            "Diana"
        };

        [HttpGet("users")]
        public IActionResult GetUsers()
        {
            return Ok(users);
        }
    }
}
