using TH1.Patterns.Singleton;

namespace TH1.Patterns.AbstractFactory
{
    public class EmailNotification : INotification
    {
        public void SendRegistrationSuccessNotification(string to)
        {
            LoggerService.Instance.Log($"Email sent to {to}: Registration successful.");
        }

        public void SendOrderSuccessNotification(string to)
        {
            LoggerService.Instance.Log($"Email sent to {to}: Order placed successfully.");
        }
    }
}
