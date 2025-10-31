using Ecommerce_API.Common;
using Ecommerce_API.DTOs.ProductDTO;
using Ecommerce_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            return StatusCode(product.StatusCode, product);
        }


        [HttpGet("filter")]
        [AllowAnonymous]
        public async Task<IActionResult> GetFilteredProducts(
            [FromQuery] string? name,
            [FromQuery] int? categoryId,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice,
            [FromQuery] bool? inStock,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string? sortBy = null,
            [FromQuery] bool descending = false)
        {
            var response = await _productService.GetFilteredProducts(
                name, categoryId, minPrice, maxPrice, inStock, page, pageSize, sortBy, descending
            );

            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _productService.AddProductAsync(model);
            return StatusCode(response.StatusCode, response);
        }


        [HttpPut("{id}")]
        [Authorize(Policy = "user")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] UpdateProductDTO model)
        {
            var response = await _productService.UpdateProductAsync(id, model);
            return StatusCode(response.StatusCode, response);
        }
    }
}
