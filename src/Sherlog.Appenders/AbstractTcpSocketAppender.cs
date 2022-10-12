using System.Collections.Generic;
using System.Net;
using TCPeasy;

namespace Sherlog.Appenders
{
    public abstract class AbstractTcpSocketAppender
    {
        readonly List<HistoryEntry> _history = new List<HistoryEntry>();

        AbstractTcpSocket _socket;

        public void Connect(IPAddress ip, int port)
        {
            var client = new TcpClientSocket();
            _socket = client;
            client.OnConnected += _ => OnConnected();
            client.Connect(ip, port);
        }

        public void Listen(int port)
        {
            var server = new TcpServerSocket();
            _socket = server;
            server.OnClientConnected += (_, _) => OnConnected();
            server.Listen(port);
        }

        public void Disconnect() => _socket.Disconnect();

        public void Send(Logger logger, LogLevel logLevel, string message)
        {
            if (IsSocketReady())
                _socket.Send(SerializeMessage(logger, logLevel, message));
            else
                _history.Add(new HistoryEntry(logger, logLevel, message));
        }

        bool IsSocketReady()
        {
            if (_socket != null)
            {
                if (_socket is TcpServerSocket server)
                    return server.Count > 0;

                if (_socket is TcpClientSocket client)
                    return client.IsConnected;
            }

            return false;
        }

        void OnConnected()
        {
            if (_history.Count > 0)
            {
                foreach (var entry in _history)
                    Send(entry.Logger, entry.LogLevel, entry.Message);

                _history.Clear();
            }
        }

        protected abstract byte[] SerializeMessage(Logger logger, LogLevel logLevel, string message);

        class HistoryEntry
        {
            public readonly Logger Logger;
            public readonly LogLevel LogLevel;
            public readonly string Message;

            public HistoryEntry(Logger logger, LogLevel logLevel, string message)
            {
                Logger = logger;
                LogLevel = logLevel;
                Message = message;
            }
        }
    }
}
