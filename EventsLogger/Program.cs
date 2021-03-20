using System;
using Common.Infrastructure.Event.MessageBroker;
using Common.Infrastructure.Nats.MessageBroker;
using EventsLogger.App.Event;
using EventsLogger.Infrastructure.Configuration;
using EventsLogger.App.Handler;
using EventsLogger.App.Logger;
using EventsLogger.Infrastructure.Event;

namespace EventsLogger
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var config = GetConfig();

            var messageBroker = new NatsMessageBroker(config);

            var eventDispatcher = new EventDispatcher(new EventChannelResolver(), messageBroker);

            var logger = new ConsoleLogger<LoggingEventHandler.SimilarityCalculatedEventPayload>();
            var handler = new LoggingEventHandler(logger);

            var subscription = eventDispatcher.Subscribe(Events.ValuatorSimilarityCalculated, handler);
            
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
