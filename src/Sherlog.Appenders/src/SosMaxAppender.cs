using System.Text;

namespace Sherlog.Appenders
{
    public class SosMaxAppender : AbstractTcpSocketAppender
    {
        protected override byte[] SerializeMessage(Logger logger, LogLevel logLevel, string message) =>
            Encoding.UTF8.GetBytes(FormatLogMessage(logLevel.ToString(), message));

        string FormatLogMessage(string logLevel, string message)
        {
            var lines = message.Split('\n');
            return lines.Length == 1
                ? $"!SOS<showMessage key=\"{logLevel}\">{ReplaceXmlSymbols(message)}</showMessage>\0"
                : $"!SOS<showFoldMessage key=\"{logLevel}\">{MultilineMessage(lines[0], message)}</showFoldMessage>\0";
        }

        string MultilineMessage(string title, string message) =>
            $"<title>{ReplaceXmlSymbols(title)}</title><message>{ReplaceXmlSymbols(message.Substring(message.IndexOf('\n') + 1))}</message>";

        string ReplaceXmlSymbols(string str) => str
            .Replace("<", "&lt;")
            .Replace(">", "&gt;")
            .Replace("&lt;", "<![CDATA[<]]>")
            .Replace("&gt;", "<![CDATA[>]]>")
            .Replace("&", "<![CDATA[&]]>");
    }
}
