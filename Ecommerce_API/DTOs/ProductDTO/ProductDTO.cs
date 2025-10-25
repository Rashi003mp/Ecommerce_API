using System.ComponentModel.DataAnnotations;

namespace Ecommerce_API.DTOs.ProductDTO
{
    public class ProductDTO
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
        
        [Required]
        public decimal Price { get; set; }

        public bool isActive { get; set; }
        public bool InStock { get; set; } = true;

        [Required]
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }
        public int CurrentStock { get; set; }

        // if there are multiple images, we can store their URLs here, or just the main image URL, intead of  List<string> we can use List<IFormFile> Images for uploading images

        [Required]
        [MinLength(1, ErrorMessage = "At least one image is required.")]
        public List<string> ImageUrls { get; set; } = new List<string>();

    }
}
