using System;
using System.Globalization;
using FluentAssertions;
using Xunit;

namespace Sherlog.Formatters.Tests
{
    public class TimestampFormatterTests
    {
        [Fact]
        public void FormatsString() => new TimestampFormatter(() => new DateTime(2000, 1, 1).ToString(CultureInfo.InvariantCulture), "{0:yyyy/MM/dd/hh:mm:ss:fff}")
            .FormatMessage(new Logger("TestLogger"), LogLevel.Debug, "test message")
            .Should().Be("01/01/2000 00:00:00 test message");
    }
}
