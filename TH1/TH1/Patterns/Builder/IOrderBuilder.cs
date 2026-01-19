using TH1.Models;
using TH1.DTOs;

namespace TH1.Patterns.Builder
{
    public interface IOrderBuilder
    {
        IOrderBuilder SetCustomerInfo(int userId);
        IOrderBuilder SetOrderItems(List<OrderItemDto> items);
        IOrderBuilder SetPaymentMethod(string paymentMethod);
        IOrderBuilder SetShippingAddress(string address);
        Order Build();
    }
}
