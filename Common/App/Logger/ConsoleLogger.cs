using System;
using System.Text.Json;

namespace Common.App.Logger
{
    public class ConsoleLogger<TMessage> : ILogger<TMessage>
    {
        private readonly struct LogRecord
        {
            public LogRecord(string timestamp, string level, TMessage payload)
            {
                this.timestamp = timestamp;
                this.level = level;
                this.payload = payload;
            }

            public string timestamp { get; }
            public string level { get; }
            public TMessage payload { get; }
        }
        
        public void Info(TMessage msg)
        {
            Console.WriteLine(CreateMessage("info", msg));
        }

        public void Error(TMessage msg)
        {
            Console.WriteLine(CreateMessage("error", msg));
        }

        public void Debug(TMessage msg)
        {
            Console.WriteLine(CreateMessage("debug", msg));
        }

        private static string CreateMessage(string level, TMessage msg)
        {
            var record = new LogRecord(
                DateTimeOffset.Now.ToUnixTimeSeconds().ToString(),
                level, 
                msg
            );
            
            return JsonSerializer.Serialize(record);
        }
    }
}