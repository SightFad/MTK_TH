using TH1.Models;
using TH1.DTOs;

namespace TH1.Patterns.Builder
{
    public class OrderBuilder : IOrderBuilder
    {
        private Order _order = new Order();

        public IOrderBuilder SetCustomerInfo(int userId)
        {
            _order.UserId = userId;
            return this;
        }

        public IOrderBuilder SetOrderItems(List<OrderItemDto> items)
        {
            _order.OrderItems = items.Select(i => new OrderItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                Price = i.Price
            }).ToList();
            _order.TotalPrice = _order.OrderItems.Sum(oi => oi.Price * oi.Quantity);
            return this;
        }

        public IOrderBuilder SetPaymentMethod(string paymentMethod)
        {
            _order.PaymentMethod = paymentMethod;
            return this;
        }

        public IOrderBuilder SetShippingAddress(string address)
        {
            _order.ShippingAddress = address;
            return this;
        }

        public Order Build()
        {
            Order result = _order;
            result.OrderDate = DateTime.UtcNow;
            _order = new Order(); // Reset builder để dùng cho lần sau
            return result;
        }   
    }
}
