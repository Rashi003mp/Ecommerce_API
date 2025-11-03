//using Ecommerce_API.Services.Interfaces;
//using Ecommerce_API.DTOs.OrderDTO;
//using Ecommerce_API.Common;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using System.Security.Claims;

//[ApiController]
//[Route("api/user/orders")]
//[Authorize(Roles = "User")] // ✅ only normal users can access
//public class UserOrderController : ControllerBase
//{
//    private readonly IOrderService _orderService;

//    public UserOrderController(IOrderService orderService)
//    {
//        _orderService = orderService;
//    }

//    [HttpPost("create")]
//    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDTO dto)
//    {
//        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
//        var result = await _orderService.CreateOrderAsync(userId, dto);
//        return StatusCode(result.StatusCode, result);
//    }

//    [HttpGet]
//    public async Task<IActionResult> GetMyOrders()
//    {
//        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
//        var result = await _orderService.GetOrdersByUserIdAsync(userId);
//        return Ok(result);
//    }

//    //[HttpGet("{orderId}")]
//    //public async Task<IActionResult> GetOrderById(int orderId)
//    //{
//    //    var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
//    //    var result = await _orderService.GetOrderByIdAsync(orderId, userId);
//    //    return Ok(result);
//    //}

//    [HttpGet("{orderId}")]
//    [Authorize(Policy = "user")]
//    public async Task<IActionResult> GetOrderById(int orderId)
//    {
//        var response = await _orderService.GetOrderbyIdAsync(orderId);
//        return StatusCode(response.StatusCode, response);
//    }
//}
