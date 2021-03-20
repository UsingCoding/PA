using System;
using System.Text;
using Common.Infrastructure.MessageBroker;
using NATS.Client;

namespace Common.Infrastructure.Nats.MessageBroker
{
    public class NatsMessageBroker : IMessageBroker
    {
        public interface IConfig
        {
            public string NatsUrl();
        }
        
        private readonly ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private readonly IConfig _config;

        public NatsMessageBroker(IConfig config)
        {
            _config = config;
            _connectionFactory = new ConnectionFactory();
        }

        public void Publish(string channel, byte[] data)
        { 
            var conn = GetConnection();
            conn.Publish(channel, data);
        }

        public IMessageBroker.ISubscription Subscribe(string channel, IMessageBroker.ISubscriber subscriber)
        {
            var conn = GetConnection();
            var subscription = conn.SubscribeAsync(channel,(sender, args) =>
            {
                subscriber.Handle(args.Message.Data);
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
                    _config.NatsUrl()
                };
                _connection = _connectionFactory.CreateConnection(opts);
            }

            return _connection;
        }
    }
}