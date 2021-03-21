using System;
using Common.App.Logger;
using Common.Infrastructure.Event.MessageBroker;
using Common.Infrastructure.Nats.MessageBroker;
using Common.Infrastructure.Redis;
using RankCalculator.Infrastructure.Configuration;
using RankCalculator.App.Handler;
using RankCalculator.Infrastructure.Event;
using RankCalculator.Infrastructure.Nats;

namespace RankCalculator
{
    static class Program
    {
        public static void Main(string[] args)
        {
            var config = GetConfig();
            var logger = new ConsoleStringLogger();
            
            var storage = new RedisStorage(config);
            var messageBroker = new NatsMessageSubscriber(config);

            var imessageBroker = new NatsMessageBroker(config);
            var eventDispatcher = new EventDispatcher(new EventChannelResolver(), imessageBroker);
            var handler = new CalculateRankMessageHandler(storage, eventDispatcher, logger);
                
             var subscription = messageBroker.Subscribe(config.ProcessingRankChannel(), config.RankCalculatorQueue(), handler.Handle);
             
             logger.Info("RankCalculator started"); 
             Console.WriteLine("Press Enter to exit");
             
             Console.ReadLine();

             subscription.Unsubscribe();
             
             logger.Info("Service successfully stopped");
        }

        private static IConfigurationProvider GetConfig()
        {
            return new Config();
        }
    }
}