using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Ecommerce_API.DTOs.ProductDTO
{
    public class UpdateProductDTO
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public bool? IsActive { get; set; }
        public bool? InStock { get; set; }
        public int? CategoryId { get; set; }
        public int? CurrentStock { get; set; }

        public List<IFormFile>? NewImages { get; set; }
        public bool ReplaceImages { get; set; } = false;
        public List<string>? ImagesToDelete { get; set; }

    }
}
