using Ecommerce_API.Models;

namespace Ecommerce_API.Entities
{
    public class Product : BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public bool InStock { get; set; } = true;
        public bool IsActive { get; set; } = true;
        public int CurrentStock { get; set; }

        public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();

    }
}
