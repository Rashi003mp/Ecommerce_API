using System.Data;

namespace Ecommerce_API.Models
{
    public class User : BaseEntity
    {
        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;

        public Roles Role { get; set; } = Roles.user;
        public bool IsBlocked { get; set; } = false;
    }
    public enum Roles
    {
        user,
        admin
    }
}


