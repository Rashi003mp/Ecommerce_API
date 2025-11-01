using Ecommerce_API.DTOs.AddressDTO;

namespace Ecommerce_API.DTOs.OrderDTO
{
    public class OrderResponseDTO
    {
         public int Id { get; set; }

      
        public int UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentStatus { get; set; }
        public string OrderStatus { get; set; }
        public string PaymentMethod { get; set; }

       
        public AddressDto Address { get; set; }

        
        public List<OrderItemDTO> Items { get; set; }


        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}
