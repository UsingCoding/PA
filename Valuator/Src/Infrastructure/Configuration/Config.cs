using Common.Infrastructure.Nats.MessageBroker;
using Common.Infrastructure.Redis;

namespace Valuator.Infrastructure.Configuration
{
    public class Config : IConfigurationProvider, RedisStorage.IConfig, NatsMessageBroker.IConfig
    {
        private const string RedisMainHost = "REDIS_MAIN_HOST";
        private const string RedisRuHost = "REDIS_RU_HOST";
        private const string RedisEuHost = "REDIS_EU_HOST";
        private const string RedisOtherHost = "REDIS_OTHER_HOST";
        private const string NatsUrl = "NATS_URL";
        
        string RedisStorage.IConfig.RedisMainDbHost() => GetFromEnv(RedisMainHost);

        string RedisStorage.IConfig.RedisRuDbHost() => GetFromEnv(RedisRuHost);

        string RedisStorage.IConfig.RedistEuHost() => GetFromEnv(RedisEuHost);

        string RedisStorage.IConfig.RedistOtherHost() => GetFromEnv(RedisOtherHost);

        string NatsMessageBroker.IConfig.NatsUrl() => GetFromEnv(NatsUrl);
        

        private string GetFromEnv(string key)
        {
            return System.Environment.GetEnvironmentVariable(key);
        }
    }
}