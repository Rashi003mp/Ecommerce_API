using Ecommerce_API.Models;

namespace Ecommerce_API.Entities
{
    public class Cart : BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public bool IsDeleted { get; set; } = false;
        public List<CartItem> Items { get; set; } = new List<CartItem>();
    }
}
