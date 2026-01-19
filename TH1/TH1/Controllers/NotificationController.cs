using Microsoft.AspNetCore.Mvc;
using TH1.Patterns.AbstractFactory;
using TH1.Patterns.Singleton;

namespace TH1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        [HttpGet("send/{type}")]
        public IActionResult SendNotification(string type, string to)
        {
            INotificationFactory factory;
            switch (type.ToLower())
            {
                case "email":
                    factory = new EmailNotificationFactory();
                    break;
                case "sms":
                    factory = new SmsNotificationFactory();
                    break;
                default:
                    return BadRequest("Invalid notification type");
            }

            var notification = factory.CreateNotification();
            notification.SendRegistrationSuccessNotification(to);
            LoggerService.Instance.Log($"Sent {type} notification to {to}.");
            return Ok($"Sent {type} notification to {to}.");
        }
    }
}
