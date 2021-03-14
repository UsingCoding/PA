using System;
using EventsLogger.App.Configuration;
using EventsLogger.App.Handler;
using EventsLogger.App.Logger;
using EventsLogger.Infrastructure.Nats;

namespace EventsLogger
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var config = GetConfig();

            var eventSource = new NatsEventSource(new NatsEventSource.Config(
                config.NatsUrl(),
                "echo"
            ));

            var logger = new ConsoleLogger<LoggingEventHandler.LogRecord>();
            var handler = new LoggingEventHandler(logger);

            var subscription = eventSource.Subscribe(handler);
            
            Console.WriteLine("EventsLogger started");
            Console.WriteLine("Press any key to stop");
            
            Console.ReadLine();
            
            subscription.Unsubscribe();
            
            Console.WriteLine("Service stopped");
        }

        private static IConfigurationProvider GetConfig()
        {
            return new Config();
        }
    }
}
