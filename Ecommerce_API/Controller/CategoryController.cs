using AutoMapper;
using Ecommerce_API.DTOs.CategoryDTO;
using Ecommerce_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        private readonly IMapper _mapper;
        public CategoryController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;


        }
        //[Authorize(Policy = "User")]
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {

            var response = await _categoryService.GetAllAsync();
            return StatusCode(response.StatusCode, response);
        }
        //[Authorize(Policy = "User")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            return StatusCode(category.StatusCode, category);

        }
        [HttpPost]
        public async Task<IActionResult> Add(CategoryDTO dto)
        {
            var result = await _categoryService.AddAsync(dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CategoryDTO dto)
        {
            var result = await _categoryService.UpdateAsync(id, dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _categoryService.DeleteAsync(id);
            return StatusCode(success.StatusCode, success);
        }
    }
}
