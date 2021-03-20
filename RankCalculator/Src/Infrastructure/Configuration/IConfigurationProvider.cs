using Common.Infrastructure.Redis;

namespace RankCalculator.Infrastructure.Configuration
{
    public interface IConfigurationProvider : RedisStorage.IConfig
    {
        public string NatsUrl();
        public string ProcessingRankChannel();
        public string RankCalculatorQueue();
    }
}