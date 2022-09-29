using FluentAssertions;
using Xunit;

namespace Sherlog.Formatters.Tests
{
    public class LogMessageFormatterTests
    {
        readonly LogMessageFormatter _formatter;
        readonly Logger _logger;

        public LogMessageFormatterTests()
        {
            _formatter = new LogMessageFormatter("[{1}] {0}: {2}");
            _logger = new Logger("TestLogger");
        }

        [Fact]
        public void FormatsString()
        {
            _formatter.FormatMessage(_logger, LogLevel.Debug, "test message")
                .Should().Be("[Debug] TestLogger: test message");
        }
    }
}
