namespace TH1.Patterns.FactoryMethod
{
    public class VNPayPaymentService : IPaymentService
    {
        public string ProcessPayment(decimal amount)
        {
            return $"Processing VNPay payment of {amount:C}";
        }
    }
}
