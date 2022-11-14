using FluentAssertions;
using Xunit;
using static Sherlog.Formatters.ColorCodeFormatter;

namespace Sherlog.Formatters.Tests
{
    public class ColorCodeFormatterTests
    {
        readonly ColorCodeFormatter _formatter;
        readonly Logger _logger;

        public ColorCodeFormatterTests()
        {
            _formatter = new ColorCodeFormatter();
            _logger = new Logger("TestLogger");
        }

        [Fact]
        public void FormatsStringUsingTraceColors() => _formatter.FormatMessage(_logger, LogLevel.Trace, "test message")
            .Should().Be($"{Esc}{CyanBackground}{Esc}{WhiteForeground}test message{Esc}{Reset}");

        [Fact]
        public void FormatsStringUsingDebugColors() => _formatter.FormatMessage(_logger, LogLevel.Debug, "test message")
            .Should().Be($"{Esc}{NoBackground}{Esc}{BlueForeground}test message{Esc}{Reset}");

        [Fact]
        public void FormatsStringUsingInfoColors() => _formatter.FormatMessage(_logger, LogLevel.Info, "test message")
            .Should().Be($"{Esc}{NoBackground}{Esc}{GreenForeground}test message{Esc}{Reset}");

        [Fact]
        public void FormatsStringUsingWarnColors() => _formatter.FormatMessage(_logger, LogLevel.Warn, "test message")
            .Should().Be($"{Esc}{NoBackground}{Esc}{YellowForeground}test message{Esc}{Reset}");

        [Fact]
        public void FormatsStringUsingErrorColors() => _formatter.FormatMessage(_logger, LogLevel.Error, "test message")
            .Should().Be($"{Esc}{RedBackground}{Esc}{WhiteForeground}test message{Esc}{Reset}");

        [Fact]
        public void FormatsStringUsingFatalColors() => _formatter.FormatMessage(_logger, LogLevel.Fatal, "test message")
            .Should().Be($"{Esc}{MagentaBackground}{Esc}{WhiteForeground}test message{Esc}{Reset}");
    }
}
