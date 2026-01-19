namespace TH1.Patterns.FactoryMethod
{
    public class PaypalPaymentService : IPaymentService
    {
        public string ProcessPayment(decimal amount)
        {
            return $"Processing PayPal payment of {amount:C}";
        }
    }
}
