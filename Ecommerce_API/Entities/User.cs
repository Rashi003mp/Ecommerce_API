using Ecommerce_API.Entities;
using System.Data;

namespace Ecommerce_API.Models
{
    public class User : BaseEntity
    {
        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;
        public Cart? Cart { get; set; }

        public ICollection<Wishlist> Wishlists { get; set; }

        public ICollection<Address> Addresses { get; set; } = new List<Address>();

        public ICollection<Order> Orders { get; set; } = new List<Order>();


        public Roles Role { get; set; } = Roles.user;
        public bool IsBlocked { get; set; } = false;
    }
    public enum Roles
    {
        user,
        admin
    }
}


