using TH1.DTOs;
using TH1.Models;
using TH1.Repositories;
using TH1.Patterns.Builder;
using TH1.Patterns.AbstractFactory;
using TH1.Patterns.Singleton;
using TH1.Patterns.Observer;
using TH1.Patterns.Strategy;

namespace TH1.Services
{
    public class OrderService : IOrderService, ISubject
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IOrderBuilder _orderBuilder;
        
        // Đã XÓA dòng private readonly IDiscountStrategy _discountStrategy;

        private readonly List<IObserver> _observers = new List<IObserver>();

        public OrderService(
            IOrderRepository orderRepository, 
            IProductRepository productRepository, 
            IOrderBuilder orderBuilder, 
            // Đã XÓA IDiscountStrategy ra khỏi tham số constructor
            IEnumerable<IObserver> injectedObservers) 
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _orderBuilder = orderBuilder;

            foreach (var observer in injectedObservers)
            {
                Attach(observer);
            }
            Attach(LoggerService.Instance); 
        }

        public void Attach(IObserver observer)
        {
            if (!_observers.Contains(observer)) _observers.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void Notify(int userId, string message) 
        {
            foreach (var observer in _observers)
            {
                observer.Update(userId, message);
            }
        }

        public Task<OrderDto> CreateOrder(int userId, CreateOrderDto createOrderDto)
        {
            var order = _orderBuilder
                .SetCustomerInfo(userId)
                .SetShippingAddress(createOrderDto.ShippingAddress)
                .SetPaymentMethod(createOrderDto.PaymentMethod)
                .SetOrderItems(createOrderDto.OrderItems)
                .Build();

            // 3. Áp dụng STRATEGY PATTERN DYNAMIC thông qua Factory
            // Lấy PromoCode từ UI gửi lên để chọn chiến lược tính tiền
            var strategy = DiscountStrategyFactory.GetStrategy(createOrderDto.PromoCode);
            order.TotalPrice = strategy.ApplyDiscount(order.TotalPrice);

            // 4. Gọi OBSERVER PATTERN để báo tin
            Notify(userId, $"Đơn hàng mới đã được tạo với tổng giá trị: {order.TotalPrice}.");

            return Task.FromResult(new OrderDto
            {
                OrderId = 0,
                UserId = order.UserId,
                OrderDate = order.OrderDate,
                TotalPrice = order.TotalPrice,
                ShippingAddress = order.ShippingAddress,
                PaymentMethod = order.PaymentMethod,
                OrderItems = order.OrderItems.Select(oi => new OrderItemDto
                {
                    ProductId = oi.ProductId,
                    Quantity = oi.Quantity,
                    Price = oi.Price
                }).ToList()
            });
        }

        // ... Các hàm GetOrderHistory và GetAllOrders giữ nguyên như cũ ...
        public async Task<IEnumerable<OrderDto>> GetOrderHistory(int userId)
        {
            var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);
            return orders.Select(o => new OrderDto
            {
                OrderId = o.OrderId, UserId = o.UserId, OrderDate = o.OrderDate, TotalPrice = o.TotalPrice,
                ShippingAddress = o.ShippingAddress, PaymentMethod = o.PaymentMethod,
                OrderItems = o.OrderItems.Select(oi => new OrderItemDto
                { ProductId = oi.ProductId, Quantity = oi.Quantity, Price = oi.Price }).ToList()
            });
        }

        public async Task<IEnumerable<OrderDto>> GetAllOrders()
        {
            var orders = await _orderRepository.GetAllOrders(); 
            return orders.Select(o => new OrderDto
            {
                OrderId = o.OrderId, UserId = o.UserId, OrderDate = o.OrderDate, TotalPrice = o.TotalPrice,
                ShippingAddress = o.ShippingAddress, PaymentMethod = o.PaymentMethod,
                OrderItems = o.OrderItems.Select(oi => new OrderItemDto
                { ProductId = oi.ProductId, Quantity = oi.Quantity, Price = oi.Price }).ToList()
            });
        }
    }
}