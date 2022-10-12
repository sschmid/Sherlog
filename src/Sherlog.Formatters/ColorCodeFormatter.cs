using System.Collections.Generic;

namespace Sherlog.Formatters
{
    public class ColorCodeFormatter
    {
        // ANSI COLOR escape codes for colors and other things.
        // You can change the color of foreground and background plus bold, italic, underline etc
        // For a complete list see http://en.wikipedia.org/wiki/ANSI_escape_code#Colors

        public const string Reset = "0m";
        public const string Esc = "\x1B[";

        public const string NoBackground = "";
        public const string BlackForeground = "30m";
        public const string BlackBackground = "40m";
        public const string RedForeground = "31m";
        public const string RedBackground = "41m";
        public const string GreenForeground = "32m";
        public const string GreenBackground = "42m";
        public const string YellowForeground = "33m";
        public const string YellowBackground = "43m";
        public const string BlueForeground = "34m";
        public const string BlueBackground = "44m";
        public const string MagentaForeground = "35m";
        public const string MagentaBackground = "45m";
        public const string CyanForeground = "36m";
        public const string CyanBackground = "46m";
        public const string WhiteForeground = "37m";
        public const string WhiteBackground = "47m";

        public class Color
        {
            public string Foreground;
            public string Background;
        }

        public readonly Dictionary<LogLevel, Color> Colors = new Dictionary<LogLevel, Color>
        {
            {LogLevel.Trace, new Color {Foreground = WhiteForeground,  Background = CyanBackground}},
            {LogLevel.Debug, new Color {Foreground = BlueForeground,   Background = NoBackground}},
            {LogLevel.Info,  new Color {Foreground = GreenForeground,  Background = NoBackground}},
            {LogLevel.Warn,  new Color {Foreground = YellowForeground, Background = NoBackground}},
            {LogLevel.Error, new Color {Foreground = WhiteForeground,  Background = RedBackground}},
            {LogLevel.Fatal, new Color {Foreground = WhiteForeground,  Background = MagentaBackground}}
        };

        public string FormatMessage(Logger logger, LogLevel logLevel, string message) =>
            $"{Esc}{Colors[logLevel].Background}{Esc}{Colors[logLevel].Foreground}{message}{Esc}{Reset}";
    }
}
