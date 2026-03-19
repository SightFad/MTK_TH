using System.Threading.Tasks;
using TH1.DTOs;
using TH1.Services;

namespace TH1.Patterns.Command
{
    public class PlaceOrderCommand : ICommand<OrderDto>
    {
        private readonly IOrderService _orderService;
        private readonly int _userId;
        private readonly CreateOrderDto _dto;

        public PlaceOrderCommand(IOrderService orderService, int userId, CreateOrderDto dto)
        {
            _orderService = orderService;
            _userId = userId;
            _dto = dto;
        }

        public async Task<OrderDto> ExecuteAsync()
        {
            // Thực thi việc gọi Service và trả về OrderDto cho Facade
            return await _orderService.CreateOrder(_userId, _dto);
        }
    }
}