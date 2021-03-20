using System;
using Common.Infrastructure.Redis;
using RankCalculator.Infrastructure.Configuration;
using RankCalculator.App.Handler;
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

            var handler = new CalculateRankMessageHandler(storage);
                
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