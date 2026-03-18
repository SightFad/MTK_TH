using TH1.Patterns.AbstractFactory;

namespace TH1.Patterns.Observer
{
    public class NotificationObserver : IObserver
    {
        private readonly INotificationFactory _notificationFactory;

        public NotificationObserver(INotificationFactory notificationFactory)
        {
            _notificationFactory = notificationFactory;
        }

        public void Update(int userId, string message)
        {
            // 1. Tạo loại thông báo dựa trên cấu hình (Email hoặc SMS)
            var notification = _notificationFactory.CreateNotification(); 

            // 2. Gửi thông báo.
            // Tạm thời chúng ta dùng userId.ToString() làm địa chỉ 'to'. 
            // Nếu muốn hoàn hảo hơn, bạn có thể inject IUserRepository vào đây để lấy Email thật.
            notification.SendOrderSuccessNotification(userId.ToString());
        }
    }
}