// Setup Sherlog
Logger.GlobalLogLevel = LogLevel.Info;
Logger.AddAppender((logger, logLevel, message) => Console.WriteLine(message));
var logger = Logger.GetLogger(nameof(Program));

// Create TcpMessageParser to handle messages from a tcp connection
var messageParser = new TcpMessageParser();
messageParser.OnMessage += (parser, bytes) => logger.Info(Encoding.UTF8.GetString(bytes));

// Listen for incoming messages on port 12345 and
// forward bytes to the messageParser to extract the actual messages.
var server = new TcpServerSocket();
server.OnReceived += (socket, client, bytes) => messageParser.Receive(bytes);
server.Listen(12345);

Console.CancelKeyPress += delegate { server.Disconnect(); };

while (true) Console.ReadLine();
