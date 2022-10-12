using System;
using System.Collections.Generic;

namespace Sherlog
{
    public partial class Logger
    {
        public static LogLevel GlobalLogLevel
        {
            get => _globalLogLevel;
            set
            {
                _globalLogLevel = value;
                foreach (var logger in Loggers.Values)
                    logger.LogLevel = value;
            }
        }

        static readonly Dictionary<string, Logger> Loggers = new Dictionary<string, Logger>();

        static LogLevel _globalLogLevel;
        static LogDelegate _appenders;

        public static void AddAppender(LogDelegate appender)
        {
            _appenders += appender;
            foreach (var logger in Loggers.Values)
                logger.OnLog += appender;
        }

        public static void RemoveAppender(LogDelegate appender)
        {
            _appenders -= appender;
            foreach (var logger in Loggers.Values)
                logger.OnLog -= appender;
        }

        public static Logger GetLogger(Type type) => GetLogger(type.FullName);

        public static Logger GetLogger(string name)
        {
            if (!Loggers.TryGetValue(name, out var logger))
            {
                logger = new Logger(name)
                {
                    LogLevel = GlobalLogLevel
                };
                logger.OnLog += _appenders;
                Loggers.Add(name, logger);
            }

            return logger;
        }

        public static void ClearLoggers() => Loggers.Clear();

        public static void ClearAppenders()
        {
            _appenders = null;
            foreach (var logger in Loggers.Values)
                logger.OnLog = null;
        }
    }
}
