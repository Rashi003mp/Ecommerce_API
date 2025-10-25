namespace Ecommerce_API.DTOs.ProductDTO
{
    public class AddProductDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; }

        // Upload image file
        public IFormFile? Image { get; set; }
    }

}
