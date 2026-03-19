using TH1.DTOs;
using TH1.Models;
using TH1.Repositories;
using TH1.Patterns.Builder;
using TH1.Patterns.AbstractFactory;
using TH1.Patterns.Singleton;
using TH1.Patterns.Observer;
using TH1.Patterns.Strategy; // Thêm Strategy namespace

namespace TH1.Services
{
    public class OrderService : IOrderService, ISubject
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IOrderBuilder _orderBuilder;
        private readonly IDiscountStrategy _discountStrategy; // Inject Strategy

        private readonly List<IObserver> _observers = new List<IObserver>();

        public OrderService(
            IOrderRepository orderRepository, 
            IProductRepository productRepository, 
            IOrderBuilder orderBuilder, 
            IDiscountStrategy discountStrategy, // Đăng ký qua DI
            IEnumerable<IObserver> injectedObservers) 
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _orderBuilder = orderBuilder;
            _discountStrategy = discountStrategy;

            // 1. Gắn các Observer từ DI
            foreach (var observer in injectedObservers)
            {
                Attach(observer);
            }

            // 2. Gắn LoggerService thủ công
            // Lưu ý: Đảm bảo class LoggerService của bạn đã "public class LoggerService : IObserver"
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

            // 3. Áp dụng STRATEGY PATTERN để tính giá đã giảm
            order.TotalPrice = _discountStrategy.ApplyDiscount(order.TotalPrice);

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