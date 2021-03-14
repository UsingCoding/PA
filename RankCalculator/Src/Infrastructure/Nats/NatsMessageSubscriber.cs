using System;
using NATS.Client;
using RankCalculator.App.Configuration;
using RankCalculator.Infrastructure.MessageSubscriber;
using ISubscription = RankCalculator.Infrastructure.MessageSubscriber.ISubscription;

namespace RankCalculator.Infrastructure.Nats
{
    public class NatsMessageSubscriber : IMessageSubscriber
    {
        private readonly ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private readonly string _natsUrl;

        public NatsMessageSubscriber(IConfigurationProvider configurationProvider)
        {
            _connectionFactory = new ConnectionFactory();
            _natsUrl = configurationProvider.NatsUrl();
        }

        public ISubscription Subscribe(string channel, string queueName, IMessageSubscriber.Handler handler)
        {
            var conn = GetConnection();
            var subscription = conn.SubscribeAsync(channel, queueName,(sender, args) =>
            {
                Console.WriteLine("Received + " + args.Message);
                handler(args.Message.Data);
            });

            subscription.Start();

            return new NatsSubscription(subscription);
        }

        public void Deconstruct()
        {
            _connection.Drain();
            _connection.Close();
            _connection.Dispose();
        }

        private IConnection GetConnection()
        {
            if (_connection == null)
            {
                var opts = ConnectionFactory.GetDefaultOptions();
                opts.Servers = new[]
                {
                    _natsUrl
                };
                _connection = _connectionFactory.CreateConnection(opts);
            }

            return _connection;
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