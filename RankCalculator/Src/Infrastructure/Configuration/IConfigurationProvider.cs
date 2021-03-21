using Common.Infrastructure.Nats.MessageBroker;
using Common.Infrastructure.Redis;

namespace RankCalculator.Infrastructure.Configuration
{
    public interface IConfigurationProvider : RedisStorage.IConfig, NatsMessageBroker.IConfig
    {
        public string ProcessingRankChannel();
        public string RankCalculatorQueue();
    }
}