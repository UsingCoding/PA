using Common.Infrastructure.Nats.MessageBroker;
using Common.Infrastructure.Redis;

namespace RankCalculator.Infrastructure.Configuration
{
    public class Config : IConfigurationProvider
    {
        private const string RedisHost = "REDIS_HOST";
        private const string NatsUrl = "NATS_URL";

        string RedisStorage.IConfig.RedisHost() => GetFromEnv(RedisHost);

        string NatsMessageBroker.IConfig.NatsUrl() => GetFromEnv(NatsUrl);

        string IConfigurationProvider.ProcessingRankChannel() => "valuator.processing.rank";

        string IConfigurationProvider.RankCalculatorQueue() => "rank_calculator";

        private string GetFromEnv(string key)
        {
            return System.Environment.GetEnvironmentVariable(key);
        }
    }
}