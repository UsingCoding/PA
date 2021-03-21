using System;
using Common.Infrastructure.Event.MessageBroker;
using Common.Infrastructure.Nats.MessageBroker;
using Common.Infrastructure.Redis;
using RankCalculator.Infrastructure.Configuration;
using RankCalculator.App.Handler;
using RankCalculator.Infrastructure.Event;
using RankCalculator.Infrastructure.Nats;

namespace RankCalculator
{
    class Program
    {
        public static void Main(string[] args)
        {
            var config = GetConfig();
            
            var storage = new RedisStorage(config);
            var messageBroker = new NatsMessageSubscriber(config);

            var imessageBroker = new NatsMessageBroker(config);
            var eventDispatcher = new EventDispatcher(new EventChannelResolver(), imessageBroker);
            var handler = new CalculateRankMessageHandler(storage, eventDispatcher);
                
             var subscription = messageBroker.Subscribe(config.ProcessingRankChannel(), config.RankCalculatorQueue(), handler.Handle);
             
             Console.WriteLine("RankCalculator started"); 
             Console.WriteLine("Press Enter to exit");
             
             Console.ReadLine();

             subscription.Unsubscribe();
             
             Console.WriteLine("Service successfully stopped");
        }

        private static IConfigurationProvider GetConfig()
        {
            return new Config();
        }
    }
}