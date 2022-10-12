namespace Sherlog.Formatters
{
    public class LogMessageFormatter
    {
        readonly string _format;

        public LogMessageFormatter(string format = "[{1}] {0}: {2}") => _format = format;

        public string FormatMessage(Logger logger, LogLevel logLevel, string message) =>
            string.Format(_format, logger.Name, logLevel, message);
    }
}
