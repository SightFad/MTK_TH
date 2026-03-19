using TH1.DTOs;
using TH1.Data;
using TH1.Models;
using TH1.Patterns.Adapter;
using TH1.Patterns.Singleton;
using TH1.Patterns.Command; // Đảm bảo import Command
using TH1.Repositories;
using TH1.Services;

namespace TH1.Patterns.Facade
{
    public interface IOrderFacade
    {
        Task<OrderResult> PlaceOrderFullProcess(int userId, CreateOrderDto dto);
    }

    public class OrderFacade : IOrderFacade
    {
        private readonly IOrderService _orderService;
        private readonly IProductRepository _productRepository;
        private readonly IPaymentService _paymentService;
        private readonly DataContext _dbContext;

        // Đã xóa INotificationFactory vì việc thông báo nay đã được Observer trong OrderService lo liệu
        public OrderFacade(
            IOrderService orderService,
            IProductRepository productRepository,
            IPaymentService paymentService,
            DataContext dbContext)
        {
            _orderService = orderService;
            _productRepository = productRepository;
            _paymentService = paymentService;
            _dbContext = dbContext;
        }

        public async Task<OrderResult> PlaceOrderFullProcess(int userId, CreateOrderDto dto)
        {
            var steps = new List<string>();

            LoggerService.Instance.Log("Toàn bộ quy trình được quản lý bởi Facade");
            steps.Add("Facade Pattern: Toàn bộ quy trình đặt hàng được điều phối bởi OrderFacade.PlaceOrderFullProcess.");

            // 1. Kiểm tra kho bằng Repository
            foreach (var item in dto.OrderItems)
            {
                if (!await _productRepository.IsInStockAsync(item.ProductId, item.Quantity))
                {
                    throw new Exception($"Sản phẩm ID {item.ProductId} không đủ hàng.");
                }
            }
            steps.Add("Repository Pattern: Đã kiểm tra tồn kho sản phẩm.");

            // 2. Sử dụng COMMAND PATTERN thay vì gọi Service trực tiếp
            var placeOrderCmd = new PlaceOrderCommand(_orderService, userId, dto);
            var calculatedOrder = await placeOrderCmd.ExecuteAsync();
            
            steps.Add("Command Pattern: Đã bọc yêu cầu đặt hàng qua PlaceOrderCommand.");
            steps.Add("Builder & Strategy Pattern: Đã build đơn hàng và áp dụng giảm giá.");
            steps.Add("Observer Pattern: NotificationObserver đã gửi tin báo đơn hàng mới.");

            // 3. Thanh toán thông qua Adapter
            var paymentResult = _paymentService.Pay(calculatedOrder.TotalPrice);
            steps.Add($"Adapter Pattern: VnPayAdapter đã gọi VnPaySdk để thanh toán. Kết quả: {paymentResult}");

            if (string.IsNullOrWhiteSpace(paymentResult))
            {
                throw new Exception("Thanh toán thất bại.");
            }

            // 4. Lưu vào Database
            var orderEntity = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                TotalPrice = calculatedOrder.TotalPrice, // Giá đã được Strategy tính lại
                ShippingAddress = dto.ShippingAddress,
                PaymentMethod = dto.PaymentMethod,
                OrderItems = dto.OrderItems.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList()
            };

            await using var transaction = await _dbContext.Database.BeginTransactionAsync();

            _dbContext.Orders.Add(orderEntity);
            await _dbContext.SaveChangesAsync();

            // 5. Trừ kho hàng 
            foreach (var item in dto.OrderItems)
            {
                await _productRepository.UpdateStockAsync(item.ProductId, -item.Quantity);
            }

            await _dbContext.SaveChangesAsync(); 
            await transaction.CommitAsync();
            steps.Add("Persistence: Đã lưu Order và OrderItems vào database Th1Db thành công.");

            var orderDto = new OrderDto
            {
                OrderId = orderEntity.OrderId, UserId = orderEntity.UserId, OrderDate = orderEntity.OrderDate,
                TotalPrice = orderEntity.TotalPrice, ShippingAddress = orderEntity.ShippingAddress,
                PaymentMethod = orderEntity.PaymentMethod,
                OrderItems = orderEntity.OrderItems.Select(oi => new OrderItemDto
                { ProductId = oi.ProductId, Quantity = oi.Quantity, Price = oi.Price }).ToList()
            };

            return new OrderResult
            {
                Order = orderDto,
                Steps = steps,
                PaymentMessage = paymentResult,
                NotificationMessage = "Observer Pattern: Email xác nhận đã được gửi ngầm thành công."
            };
        }
    }
}