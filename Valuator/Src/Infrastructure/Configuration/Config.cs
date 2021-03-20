using Common.Infrastructure.Redis;

namespace Valuator.Infrastructure.Configuration
{
    public class Config : IConfigurationProvider, RedisStorage.IConfig
    {
        private const string RedisHost = "REDIS_HOST";
        private const string NatsUrl = "NATS_URL";

        string RedisStorage.IConfig.RedisHost() => GetFromEnv(RedisHost);

        string IConfigurationProvider.NatsUrl() => GetFromEnv(NatsUrl);
        

        private string GetFromEnv(string key)
        {
            return System.Environment.GetEnvironmentVariable(key);
        }
    }
}