using FluentAssertions;
using Xunit;

namespace Sherlog.Formatters.Tests
{
    public class LogMessageFormatterTests
    {
        [Fact]
        public void FormatsString() => new LogMessageFormatter("[{1}] {0}: {2}")
            .FormatMessage(new Logger("TestLogger"), LogLevel.Debug, "test message")
            .Should().Be("[Debug] TestLogger: test message");
    }
}
