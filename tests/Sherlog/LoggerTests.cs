using System;
using FluentAssertions;
using Xunit;

namespace Sherlog.Tests
{
    public class LoggerTests
    {
        const string Message = "test message";

        readonly Logger _logger;

        public LoggerTests()
        {
            _logger = new Logger("TestLogger");
        }

        [Theory]
        [InlineData(LogLevel.On, true, true, true, true, true, true)]
        [InlineData(LogLevel.Trace, true, true, true, true, true, true)]
        [InlineData(LogLevel.Debug, false, true, true, true, true, true)]
        [InlineData(LogLevel.Info, false, false, true, true, true, true)]
        [InlineData(LogLevel.Warn, false, false, false, true, true, true)]
        [InlineData(LogLevel.Error, false, false, false, false, true, true)]
        [InlineData(LogLevel.Fatal, false, false, false, false, false, true)]
        [InlineData(LogLevel.Off, false, false, false, false, false, false)]
        public void LogLevels(LogLevel logLevel, bool trace, bool debug, bool info, bool warn, bool error, bool fatal)
        {
            _logger.LogLevel = logLevel;
            AssertLogLevel(_logger.Trace, LogLevel.Trace, trace);
            AssertLogLevel(_logger.Debug, LogLevel.Debug, debug);
            AssertLogLevel(_logger.Info, LogLevel.Info, info);
            AssertLogLevel(_logger.Warn, LogLevel.Warn, warn);
            AssertLogLevel(_logger.Error, LogLevel.Error, error);
            AssertLogLevel(_logger.Fatal, LogLevel.Fatal, fatal);

            void AssertLogLevel(Action<string> logMethod, LogLevel logLvl, bool shouldLog)
            {
                var didLog = false;
                var eventLogLevel = LogLevel.Off;
                string eventMessage = null;
                Logger eventLogger = null;
                _logger.OnLog += (logger, level, msg) =>
                {
                    didLog = true;
                    eventLogger = logger;
                    eventLogLevel = level;
                    eventMessage = msg;
                };

                logMethod(Message);

                didLog.Should().Be(shouldLog);

                if (shouldLog)
                {
                    eventLogger.Should().BeSameAs(_logger);
                    eventMessage.Should().Be(Message);
                    eventLogLevel.Should().Be(logLvl);
                }
                else
                {
                    eventMessage.Should().BeNull();
                    eventLogLevel.Should().Be(LogLevel.Off);
                    eventLogger.Should().BeNull();
                }
            }
        }

        [Fact]
        public void AssertDoesNotThrowWhenConditionIsTrue()
        {
            _logger.Assert(true, "success");
        }

        [Fact]
        public void AssertThrowsWhenConditionIsFalse()
        {
            FluentActions.Invoking(() => _logger.Assert(false, "fail"))
                .Should().Throw<SherlogAssertException>();
        }

        [Fact]
        public void ResetsOnLog()
        {
            var didLog = 0;
            _logger.OnLog += (logger, level, s) => didLog += 1;
            _logger.Reset();
            _logger.Info("test message");
            didLog.Should().Be(0);
        }
    }
}
