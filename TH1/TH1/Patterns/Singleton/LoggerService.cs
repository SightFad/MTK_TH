using TH1.Patterns.Observer;

namespace TH1.Patterns.Singleton
{
    public sealed class LoggerService : IObserver
    {
        private static readonly Lazy<LoggerService> lazy = new Lazy<LoggerService>(() => new LoggerService());
        private readonly string logFile = "log.txt";

        public static LoggerService Instance { get { return lazy.Value; } }

        private LoggerService()
        {
        }

        public void Log(string message)
        {
            File.AppendAllText(logFile, $"[{DateTime.Now:dd-MM-yyyy HH:mm:ss}] {message}{Environment.NewLine}");
        }

        public void Update(int userId, string message)
        {
            Log($"[Sự kiện hệ thống] UserID {userId}: {message}");
        }
    }
}
