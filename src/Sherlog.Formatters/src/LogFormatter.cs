namespace Sherlog.Formatters
{
    public delegate string LogFormatter(Logger logger, LogLevel logLevel, string message);
}
