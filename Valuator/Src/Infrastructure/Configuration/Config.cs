using Common.Infrastructure.Nats.MessageBroker;
using Common.Infrastructure.Redis;

namespace Valuator.Infrastructure.Configuration
{
    public class Config : IConfigurationProvider, RedisStorage.IConfig, NatsMessageBroker.IConfig
    {
        private const string RedisHost = "REDIS_HOST";
        private const string NatsUrl = "NATS_URL";

        string RedisStorage.IConfig.RedisHost() => GetFromEnv(RedisHost);

        string NatsMessageBroker.IConfig.NatsUrl() => GetFromEnv(NatsUrl);
        

        private string GetFromEnv(string key)
        {
            return System.Environment.GetEnvironmentVariable(key);
        }
    }
}