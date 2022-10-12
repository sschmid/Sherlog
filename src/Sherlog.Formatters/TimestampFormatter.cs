using System;

namespace Sherlog.Formatters
{
    public class TimestampFormatter
    {
        readonly string _timeFormat;

        public TimestampFormatter(string timeFormat = "{0:yyyy/MM/dd/hh:mm:ss:fff}") => _timeFormat = timeFormat;

        public string FormatMessage(Logger logger, LogLevel logLevel, string message) =>
            $"{string.Format(_timeFormat, DateTime.Now)} {message}";
    }
}
