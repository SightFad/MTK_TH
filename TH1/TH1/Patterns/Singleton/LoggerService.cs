namespace TH1.Patterns.Singleton
{
    public sealed class LoggerService
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
    }
}
