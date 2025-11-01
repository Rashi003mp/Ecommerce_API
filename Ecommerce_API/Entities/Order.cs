using Ecommerce_API.Entities;
using Ecommerce_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_API.Entities
{
    public class Order : BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }

        [Precision(18, 2)]
        public decimal TotalAmount { get; set; }

        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;
        public PaymentMethod PaymentMethod { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;

        public int AddressId { get; set; }
        public Address Address { get; set; }

        // Razorpay order ID (required for online payments)
        public string? RazorpayOrderId { get; set; }

        // Payment ID returned by Razorpay after payment
        public string? PaymentId { get; set; }
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();


    }
    public enum PaymentStatus
    {
        Pending,
        Completed,
        Failed,
        Refunded
    }

    public enum PaymentMethod
    {
        CashOnDelivery,
        Razorpay
    }

    public enum OrderStatus
    {
        Pending,
        Processing,
        Shipped,
        Delivered,
        Cancelled
    }

}