using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce_API.DTOs.ProductDTO
{
    public class CreateProductDTO
    {
        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public int CurrentStock { get; set; } = 0;

        // Multiple images upload
        [Required]
        public List<IFormFile> Images { get; set; } = new List<IFormFile>();

        // Index of the main image (example: 0 = first image, 1 = second)
        public int MainImageIndex { get; set; } = 0;
    }
}
