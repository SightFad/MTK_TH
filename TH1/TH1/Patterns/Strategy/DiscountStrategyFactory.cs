namespace TH1.Patterns.Strategy
{
    public static class DiscountStrategyFactory
    {
        // Thêm dấu ? vào string? promoCode
        public static IDiscountStrategy GetStrategy(string? promoCode)
        {
            // Hàm string.Equals xử lý null rất an toàn, nếu promoCode là null nó sẽ tự trả về false
            if (string.Equals(promoCode, "vip20", StringComparison.OrdinalIgnoreCase))
            {
                return new PercentDiscount(20);
            }
            
            // Nếu gửi "none", gửi mã sai, hoặc không gửi gì (null), đều không giảm giá
            return new NoDiscount(); 
        }
    }
}