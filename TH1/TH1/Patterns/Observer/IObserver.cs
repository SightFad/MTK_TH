namespace TH1.Patterns.Observer
{
    public interface IObserver
    {
        // Thêm userId để Notification biết gửi cho ai
        void Update(int userId, string message); 
    }
}