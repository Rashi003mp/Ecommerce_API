using Ecommerce_API.DTOs.AddressDTO;
using Ecommerce_API.Entities;

namespace Ecommerce_API.DTOs.OrderDTO
{
    public class CreateOrderDTO
    {
        // Either existing address
        public int AddressId { get; set; }

        // Or provide new address
        public AddressDto? NewAddress { get; set; }

        public PaymentMethod PaymentMethod { get; set; }
    }

    public class BuyNowDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; } = 1;
    }
}
