using System.Text;
using TCPeasy;

namespace Sherlog.Appenders
{
    public class TcpSocketAppender : AbstractTcpSocketAppender
    {
        protected override byte[] SerializeMessage(Logger logger, LogLevel logLevel, string message) =>
            TcpMessageParser.WrapMessage(Encoding.UTF8.GetBytes(message));
    }
}
