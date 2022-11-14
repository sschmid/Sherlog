Reset("Create Logger And Appender Sample");
CreateLoggerAndAppenderSample();

Reset("Colorful Messages Sample");
ColorfulMessagesSample();

Reset("Formatter Sample");
FormatterSample();

Reset("Log Level Sample");
LogLevelSample();

Reset("Send Message Via Tcp Connection Sample");
SendMessageViaTcpConnectionSample();

void CreateLoggerAndAppenderSample()
{
    // Create a new logger with type
    var programLogger = Logger.GetLogger(typeof(Program));
    programLogger.Info("No appenders set yet, this message doesn't do anything");

    // Add global appender to all existing and future loggers
    Logger.AddAppender((logger, level, message) => Console.WriteLine(message));
    programLogger.Info($"Greetings from {programLogger.Name}!");

    // Create a new logger with name which will inherit global appenders
    var myLogger = Logger.GetLogger("MyLogger");
    myLogger.Info($"Greetings from {myLogger.Name}!");

    // Retrieve the same logger instance by name
    var loggerByName = Logger.GetLogger("MyLogger");
    myLogger.Assert(loggerByName == myLogger, "loggers should be same");
}

void ColorfulMessagesSample()
{
    // Add colorful console messages
    var consoleAppender = new ConsoleAppender(new Dictionary<LogLevel, ConsoleColor>
    {
        {LogLevel.Trace, ConsoleColor.Cyan},
        {LogLevel.Debug, ConsoleColor.Blue},
        {LogLevel.Info, ConsoleColor.White},
        {LogLevel.Warn, ConsoleColor.Yellow},
        {LogLevel.Error, ConsoleColor.Red},
        {LogLevel.Fatal, ConsoleColor.Magenta},
    });

    Logger.AddAppender(consoleAppender.WriteLine);

    // You will see messages with color
    var logger = Logger.GetLogger(typeof(Program));
    logger.Trace("This is a message using logger.Trace()");
    logger.Debug("This is a message using logger.Debug()");
    logger.Info("This is a message using logger.Info()");
    logger.Warn("This is a message using logger.Warn()");
    logger.Error("This is a message using logger.Error()");
    logger.Fatal("This is a message using logger.Fatal()");

    try
    {
        logger.Assert(1 == 2, "One is not Two");
    }
    catch (Exception e)
    {
        logger.Error(e.Message);
    }
}

void FormatterSample()
{
    // Add more detailed message formatting
    var messageFormatter = new LogMessageFormatter();
    var timestampFormatter = new TimestampFormatter(() => DateTime.UtcNow.ToString(CultureInfo.InvariantCulture));
    var consoleAppender = new ConsoleAppender(new Dictionary<LogLevel, ConsoleColor>
    {
        {LogLevel.Trace, ConsoleColor.Cyan},
        {LogLevel.Debug, ConsoleColor.Blue},
        {LogLevel.Info, ConsoleColor.White},
        {LogLevel.Warn, ConsoleColor.Yellow},
        {LogLevel.Error, ConsoleColor.Red},
        {LogLevel.Fatal, ConsoleColor.Magenta},
    });

    Logger.AddAppender((logger, level, message) =>
    {
        message = messageFormatter.FormatMessage(logger, level, message);
        message = timestampFormatter.FormatMessage(logger, level, message);
        consoleAppender.WriteLine(logger, level, message);
    });

    // You will see messages with color, timestamp, log level and logger name
    var logger = Logger.GetLogger(typeof(Program));
    logger.Trace("This is a message using logger.Trace()");
    logger.Debug("This is a message using logger.Debug()");
    logger.Info("This is a message using logger.Info()");
    logger.Warn("This is a message using logger.Warn()");
    logger.Error("This is a message using logger.Error()");
    logger.Fatal("This is a message using logger.Fatal()");
}

void LogLevelSample()
{
    Logger.GlobalLogLevel = LogLevel.Warn;
    Logger.AddAppender((logger, level, message) => Console.WriteLine(message));

    // You will only see messages with a log level of LogLevel.Warn or higher
    var logger = Logger.GetLogger(typeof(Program));
    logger.Trace("This is a message using logger.Trace()");
    logger.Debug("This is a message using logger.Debug()");
    logger.Info("This is a message using logger.Info()");
    logger.Warn("This is a message using logger.Warn()");
    logger.Error("This is a message using logger.Error()");
    logger.Fatal("This is a message using logger.Fatal()");
}

void SendMessageViaTcpConnectionSample()
{
    // Send messages via tcp (localhost:12345)
    var socketAppender = new TcpSocketAppender();
    socketAppender.Connect(IPAddress.Loopback, 12345);

    // TcpSocketAppender also uses Sherlog under the hood and logs sending messages
    // with logger.Debug(message). Set its log level higher than LogLevel.Debug
    // to avoid stack overflow.
    Logger.GetLogger(typeof(TcpClientSocket)).LogLevel = LogLevel.Info;

    var messageFormatter = new LogMessageFormatter();
    var timestampFormatter = new TimestampFormatter(() => DateTime.UtcNow.ToString(CultureInfo.InvariantCulture));
    var colorCodeFormatter = new ColorCodeFormatter();
    Logger.AddAppender((logger, level, message) =>
    {
        message = messageFormatter.FormatMessage(logger, level, message);
        message = timestampFormatter.FormatMessage(logger, level, message);
        message = colorCodeFormatter.FormatMessage(logger, level, message);
        socketAppender.Send(logger, level, message);
    });

    // Run SherlogServer sample to receive messages on localhost:12345
    // dotnet run --project samples/SherlogServer/SherlogServer.csproj
    var logger = Logger.GetLogger(typeof(Program));
    logger.Trace("This is a message using logger.Trace()");
    logger.Debug("This is a message using logger.Debug()");
    logger.Info("This is a message using logger.Info()");
    logger.Warn("This is a message using logger.Warn()");
    logger.Error("This is a message using logger.Error()");
    logger.Fatal("This is a message using logger.Fatal()");

    Console.WriteLine("Press any key to continue . . .");
    Console.ReadKey(true);
}

void Reset(string message)
{
    Logger.ClearAppenders();
    Logger.GlobalLogLevel = LogLevel.On;
    Logger.AddAppender((logger, level, message) => Console.WriteLine(message));
    var logger = Logger.GetLogger(typeof(Program));
    logger.Info(string.Empty);
    logger.Info("############################################################");
    logger.Info($"# {message}");
    logger.Info("############################################################");
    Logger.ClearAppenders();
}
