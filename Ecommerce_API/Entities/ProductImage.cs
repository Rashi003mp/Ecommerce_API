using Ecommerce_API.Models;

namespace Ecommerce_API.Entities
{
    public class ProductImage : BaseEntity
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public string ImageUrl { get; set; } = null!;  // Cloudinary URL
        public string PublicId { get; set; } = null!;  // Cloudinary public ID
        public bool IsMain { get; set; } = false;
    }

}
