using System;

namespace Sherlog.Formatters
{
    public class TimestampFormatter
    {
        readonly Func<string> _timeDelegate;
        readonly string _timeFormat;

        public TimestampFormatter(Func<string> timeDelegate, string timeFormat = "{0:yyyy/MM/dd/hh:mm:ss:fff}")
        {
            _timeDelegate = timeDelegate;
            _timeFormat = timeFormat;
        }

        public string FormatMessage(Logger logger, LogLevel logLevel, string message) =>
            $"{string.Format(_timeFormat, _timeDelegate())} {message}";
    }
}
