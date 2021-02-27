using Microsoft.Extensions.Logging;
using NATS.Client;
using Valuator.Common.App.Configuration;
using Valuator.Infrastructure.MessageBroker;

namespace Valuator.Infrastructure.Nats
{
    public class NatsMessageBroker : IMessageBroker
    {
        private readonly ILogger<NatsMessageBroker> _logger;
        private readonly ConnectionFactory _connectionFactory;
        private readonly string _natsUrl;

        public NatsMessageBroker(ILogger<NatsMessageBroker> logger, IConfigurationProvider configurationProvider)
        {
            _logger = logger;
            _connectionFactory = new ConnectionFactory();
            _natsUrl = configurationProvider.NatsUrl();
        }

        public void Publish(string channel, byte[] data)
        {
            using var conn = CreateConnection();
            conn.Publish(channel, data);
            _logger.LogInformation("To channel - {Channel} send data: {Data}", channel, data);
        }

        private IConnection CreateConnection()
        {
            Options opts = ConnectionFactory.GetDefaultOptions();
            opts.Servers = new[]
            {
                _natsUrl
            };

            return _connectionFactory.CreateConnection(opts);
        }
    }
}