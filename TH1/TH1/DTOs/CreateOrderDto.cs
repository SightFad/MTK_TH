using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TH1.DTOs
{
    public class CreateOrderDto
    {
        public string ShippingAddress { get; set; }
        public string PaymentMethod { get; set; }
        public string? PromoCode { get; set; } // Thêm trường này
        public List<OrderItemDto> OrderItems { get; set; }
    }
}
