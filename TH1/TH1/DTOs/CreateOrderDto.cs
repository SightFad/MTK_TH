using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TH1.DTOs
{
    public class CreateOrderDto
    {
        [Required]
        public string ShippingAddress { get; set; }

        [Required]
        public string PaymentMethod { get; set; } // "Cash", "Paypal", "VNPay"

        [Required]
        public List<OrderItemDto> OrderItems { get; set; }
    }
}
