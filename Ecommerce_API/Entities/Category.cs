using Ecommerce_API.Models;

namespace Ecommerce_API.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = null!;

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
