using System;
using RankCalculator.App.Configuration;
using RankCalculator.App.Handler;
using RankCalculator.Infrastructure.Nats;
using RankCalculator.Infrastructure.Redis;

namespace RankCalculator
{
    class Program
    {
        public static void Main(string[] args)
        {
            var config = new Config();
            
            var storage = new RedisStorage(config);
            var messageBroker = new NatsMessageSubscriber(config);

            var handler = new CalculateRankMessageHandler(storage);
                
             var subscription = messageBroker.Subscribe("valuator.processing.rank", "rank_calculator", handler.Handle);
             
             Console.WriteLine("RankCalculator started"); 
             Console.WriteLine("Press Enter to exit");
             
             Console.ReadLine();

             subscription.Unsubscribe();
             
             Console.WriteLine("Service successfully stopped");
        }
    }
}