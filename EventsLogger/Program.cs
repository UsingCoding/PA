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
            
            var eventDispatcher = ResolveEventDispatcher(config);

            var similarityCalculatedEventLogger = new ConsoleLogger<LoggingEventHandler.SimilarityCalculatedEventPayload>();
            var rankCalculatedEventLogger = new ConsoleLogger<LoggingEventHandler.RankCalculatedEventPayload>();
            var logger = new ConsoleStringLogger();

            var handler = new LoggingEventHandler(similarityCalculatedEventLogger, rankCalculatedEventLogger, logger);

            var similarityCalculatedEventSubscription = eventDispatcher.Subscribe(Events.ValuatorSimilarityCalculated, handler);
            var rankCalculatedEventSubscription = eventDispatcher.Subscribe(Events.RankCalculatorRankCalculated, handler);
            
            logger.Info("EventsLogger started");

            Console.ReadLine();
            
            similarityCalculatedEventSubscription.Unsubscribe();
            rankCalculatedEventSubscription.Unsubscribe();
            
            logger.Info("Service stopped");
        }

        private static IConfigurationProvider GetConfig()
        {
            return new Config();
        }

        private static EventDispatcher ResolveEventDispatcher(NatsMessageBroker.IConfig config)
        {
            var messageBroker = new NatsMessageBroker(config);

            return new EventDispatcher(new EventChannelResolver(), messageBroker);
        }
    }
}
