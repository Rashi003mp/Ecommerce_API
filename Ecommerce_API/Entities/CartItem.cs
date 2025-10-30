using Ecommerce_API.Models;

namespace Ecommerce_API.Entities
{
    public class CartItem : BaseEntity
    {

        public int CartId { get; set; }
        public Cart Cart { get; set; }

        public Product Product { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; } = 1;
        public string ImageUrl { get; set; }
    }
}
