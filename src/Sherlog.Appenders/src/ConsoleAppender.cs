using System.Collections.Generic;
using System;

namespace Sherlog.Appenders
{
    public class ConsoleAppender
    {
        readonly Dictionary<LogLevel, ConsoleColor> _consoleColors;

        public ConsoleAppender(Dictionary<LogLevel, ConsoleColor> consoleColors)
        {
            _consoleColors = consoleColors;
        }

        public void WriteLine(Logger logger, LogLevel logLevel, string message)
        {
            if (_consoleColors.TryGetValue(logLevel, out var color))
            {
                Console.ForegroundColor = color;
                Console.WriteLine(message);
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine(message);
            }
        }
    }
}
