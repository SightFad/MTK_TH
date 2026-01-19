namespace TH1.Patterns.AbstractFactory
{
    public class SmsNotificationFactory : INotificationFactory
    {
        public INotification CreateNotification()
        {
            return new SmsNotification();
        }
    }
}
