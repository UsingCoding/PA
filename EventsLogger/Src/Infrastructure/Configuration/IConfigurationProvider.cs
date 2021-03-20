using Common.Infrastructure.Nats.MessageBroker;

namespace EventsLogger.Infrastructure.Configuration
{
    public interface IConfigurationProvider : NatsMessageBroker.IConfig
    {
        
    }
}