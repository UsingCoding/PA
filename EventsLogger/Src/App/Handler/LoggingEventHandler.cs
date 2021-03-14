using System.Text;
using EventsLogger.App.Logger;
using EventsLogger.Infrastructure.EventSource;

namespace EventsLogger.App.Handler
{
    public class LoggingEventHandler : IEventSource.ISubscriber
    {
        public struct LogRecord
        {
            public string payload;

            public LogRecord(string payload)
            {
                this.payload = payload;
            }
        }

        private readonly ILogger<LogRecord> _logger;

        public LoggingEventHandler(ILogger<LogRecord> logger)
        {
            _logger = logger;
        }

        public void Handle(IEventSource.Event e)
        {
            _logger.Info(new LogRecord(Encoding.UTF8.GetString(e.Payload)));
        }
    }
}