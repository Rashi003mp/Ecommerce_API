namespace Ecommerce_API.DTOs.ProductDTO
{
    public class CreateProductDTO
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public int CurrentStock { get; set; }
        public bool InStock { get; set; } = true;

        // Frontend will upload images here
        public List<IFormFile> Images { get; set; } = new List<IFormFile>();

    }
}
