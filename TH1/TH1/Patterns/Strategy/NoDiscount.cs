namespace TH1.Patterns.Strategy
{
    public class NoDiscount : IDiscountStrategy 
    {
        public decimal ApplyDiscount(decimal totalAmount) => totalAmount;
    }
}