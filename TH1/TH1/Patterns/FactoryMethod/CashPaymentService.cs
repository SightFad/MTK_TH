namespace TH1.Patterns.FactoryMethod
{
    public class CashPaymentService : IPaymentService
    {
        public string ProcessPayment(decimal amount)
        {
            return $"Processing cash payment of {amount:C}";
        }
    }
}
