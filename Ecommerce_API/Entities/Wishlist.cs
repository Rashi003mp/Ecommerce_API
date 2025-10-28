using System.ComponentModel.DataAnnotations;

namespace Ecommerce_API.Entities
{
    public class Wishlist
    {
       
            [Key]
            public int Id { get; set; }

            // Foreign Key - User
            public string UserId { get; set; }

            // Foreign Key - Product
            public int ProductId { get; set; }
            public Product Product { get; set; }

            public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
       
    }
}
