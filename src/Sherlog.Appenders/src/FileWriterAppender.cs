using System.IO;

namespace Sherlog.Appenders
{
    public class FileWriterAppender
    {
        readonly object _lock = new object();
        readonly string _filePath;

        public FileWriterAppender(string filePath) => _filePath = filePath;

        public void WriteLine(Logger logger, LogLevel logLevel, string message)
        {
            lock (_lock)
            {
                using var writer = new StreamWriter(_filePath, true);
                writer.WriteLine(message);
            }
        }

        public void ClearFile()
        {
            lock (_lock)
            {
                using var writer = new StreamWriter(_filePath, false);
                writer.Write(string.Empty);
            }
        }
    }
}
