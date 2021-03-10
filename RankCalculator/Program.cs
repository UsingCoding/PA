using System;
using RankCalculator.App.Configuration;
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
            var messageBroker = new NatsMessageBroker(config);
            
             var subscription = messageBroker.Subscribe("valuator.calculate_rank");
             
             Console.WriteLine("Press Enter to exit");
             Console.ReadLine();
             
             subscription.Unsubscribe();
        }
        
    }
}