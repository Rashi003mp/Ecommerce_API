using Ecommerce_API.Models;

namespace Ecommerce_API.Entities
{
    public class Address : BaseEntity
    {
        public int UserId { get; set; }       // FK to User

        public string FullName { get; set; } = string.Empty;  // Receiver name
        public string PhoneNumber { get; set; } = string.Empty;

        public string AddressLine1 { get; set; } = string.Empty;
        public string? AddressLine2 { get; set; }
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;

        public bool IsDefault { get; set; } = false;          // Mark as default address

        // Navigation property
        public User? User { get; set; }
    }

}
