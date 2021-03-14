using System;
using System.Text;
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
            var messageBroker = new NatsMessageSubscriber(config);
                
             var subscription = messageBroker.Subscribe("valuator.processing.rank", "rank_calculator", data =>
             {
                 var taskId = Encoding.UTF8.GetString(data);
                 
                 Console.WriteLine("Processing task - {0}", taskId);
                 
                 
             });
             
             Console.WriteLine("RankCalculator started"); 
             Console.WriteLine("Press Enter to exit");
             
             Console.ReadLine();

             subscription.Unsubscribe();
             
             Console.WriteLine("Service successfully stopped");
        }
    }
}