using EventsLogger.Infrastructure.EventSource;
using NATS.Client;

namespace EventsLogger.Infrastructure.Nats
{
    public class NatsEventSource : IEventSource
    {
        public readonly struct Config
        {
            public Config(string natsUrl, string channel)
            {
                NatsUrl = natsUrl;
                Channel = channel;
            }

            public string NatsUrl { get; }
            public string Channel { get; }
        }

        private readonly ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private readonly Config _config;

        public NatsEventSource(Config config)
        {
            _connectionFactory = new ConnectionFactory();
            _config = config;
        }
        
        public IEventSource.ISubscription Subscribe(IEventSource.ISubscriber subscriber)
        {
            var conn = GetConnection();
            var subscription = conn.SubscribeAsync(_config.Channel,(sender, args) =>
            {
                subscriber.Handle(new IEventSource.Event("type", args.Message.Data));
            });

            subscription.Start();

            return new Subscription(subscription);   
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
                    _config.NatsUrl
                };
                _connection = _connectionFactory.CreateConnection(opts);
            }

            return _connection;
        }

        private class Subscription : IEventSource.ISubscription
        {
            private readonly IAsyncSubscription _subscription;

            public Subscription(IAsyncSubscription subscription)
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
    
}