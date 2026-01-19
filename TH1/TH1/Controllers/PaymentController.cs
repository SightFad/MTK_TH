using Microsoft.AspNetCore.Mvc;
using TH1.Patterns.FactoryMethod;
using TH1.Patterns.Singleton;

namespace TH1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        [HttpGet("pay/{method}")]
        public IActionResult Pay(string method, decimal amount)
        {
            PaymentServiceFactory factory;
            switch (method.ToLower())
            {
                case "cash":
                    factory = new CashPaymentFactory();
                    break;
                case "paypal":
                    factory = new PaypalPaymentFactory();
                    break;
                case "vnpay":
                    factory = new VNPayPaymentFactory();
                    break;
                default:
                    return BadRequest("Invalid payment method");
            }

            var paymentService = factory.CreatePaymentService();
            var result = paymentService.ProcessPayment(amount);
            LoggerService.Instance.Log(result);
            return Ok(result);
        }
    }
}
