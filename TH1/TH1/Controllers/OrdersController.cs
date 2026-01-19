using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TH1.DTOs;
using TH1.Services;
using TH1.Patterns.Singleton;

namespace TH1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
            LoggerService.Instance.Log("OrdersController initialized.");
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderDto createOrderDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            try
            {
                var order = await _orderService.CreateOrder(userId, createOrderDto);
                LoggerService.Instance.Log($"Order {order.OrderId} created for user {userId}.");
                return Ok(order);
            }
            catch (Exception ex)
            {
                LoggerService.Instance.Log($"Order creation failed for user {userId}: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetOrderHistory()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var orders = await _orderService.GetOrderHistory(userId);
            return Ok(orders);
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetAllOrders()
        {
            // In a real app, you would implement this in the OrderService
            // For now, this is just a placeholder for the admin functionality
             LoggerService.Instance.Log($"Admin requesting all orders.");
            return Ok("Not Implemented");
        }
    }
}
