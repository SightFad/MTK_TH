namespace TH1.Patterns.Strategy
{
    public class PercentDiscount : IDiscountStrategy
    {
        private readonly decimal _percentage;
        public PercentDiscount(decimal percentage) => _percentage = percentage;
        public decimal ApplyDiscount(decimal totalAmount) => totalAmount - (totalAmount * _percentage / 100);
    }
}