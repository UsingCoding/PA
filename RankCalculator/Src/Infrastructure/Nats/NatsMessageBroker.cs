using System;
using NATS.Client;
using RankCalculator.App.Configuration;
using RankCalculator.Infrastructure.MessageBroker;
using ISubscription = RankCalculator.Infrastructure.MessageBroker.ISubscription;

namespace RankCalculator.Infrastructure.Nats
{
    public class NatsMessageBroker : IMessageBroker
    {
        private readonly ConnectionFactory _connectionFactory;
        private readonly string _natsUrl;

        public NatsMessageBroker(IConfigurationProvider configurationProvider)
        {
            _connectionFactory = new ConnectionFactory();
            _natsUrl = configurationProvider.NatsUrl();
        }

        public void Publish(string channel, byte[] data)
        {
            using var conn = CreateConnection();
            conn.Publish(channel, data);
        }

        public ISubscription Subscribe(string channel)
        {
            using var conn = CreateConnection();
            var subscription = conn.SubscribeAsync(channel);
            subscription.MessageHandler += (sender, args) =>
            {
                Console.WriteLine("Received + " + args.Message);
            };
            
            subscription.Start();

            return new NatsSubscription(subscription);
        }

        private IConnection CreateConnection()
        {
            var opts = ConnectionFactory.GetDefaultOptions();
            opts.Servers = new[]
            {
                _natsUrl
            };

            return _connectionFactory.CreateConnection(opts);
        }
    }
    
    public class NatsSubscription : ISubscription
    {
        private IAsyncSubscription _subscription;

        public NatsSubscription(IAsyncSubscription subscription)
        {
            _subscription = subscription;
        }

        public void Unsubscribe()
        {
            _subscription.Unsubscribe();
        }

        public void Deconstruct(out IAsyncSubscription subscription)
        {
            subscription = _subscription;
            
            subscription.Unsubscribe();
        }
    }
}