using System;

namespace Sherlog
{
    public delegate void LogDelegate(Logger logger, LogLevel logLevel, string message);

    public partial class Logger
    {
        public event LogDelegate OnLog;

        public readonly string Name;

        public LogLevel LogLevel;

        public Logger(string name) => Name = name;

        public void Trace(string message) => Log(LogLevel.Trace, message);
        public void Debug(string message) => Log(LogLevel.Debug, message);
        public void Info (string message) => Log(LogLevel.Info, message);
        public void Warn (string message) => Log(LogLevel.Warn, message);
        public void Error(string message) => Log(LogLevel.Error, message);
        public void Fatal(string message) => Log(LogLevel.Fatal, message);

        public void Assert(bool condition, string message)
        {
            if (!condition)
                throw new SherlogAssertException(message);
        }

#if SHERLOG_OFF
        [System.Diagnostics.Conditional("false")]
#endif
        void Log(LogLevel logLvl, string message)
        {
            if (logLvl >= LogLevel)
                OnLog?.Invoke(this, logLvl, message);
        }

        public void Reset() => OnLog = null;
    }

    public class SherlogAssertException : Exception
    {
        public SherlogAssertException(string message) : base(message) { }
    }
}
