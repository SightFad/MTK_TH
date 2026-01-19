namespace TH1.Patterns.AbstractFactory
{
    public interface INotification
    {
        void SendRegistrationSuccessNotification(string to);
        void SendOrderSuccessNotification(string to);
    }
}
