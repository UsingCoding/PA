namespace RankCalculator.App.Configuration
{
    public class Config : IConfigurationProvider
    {
        private const string RedisHost = "REDIS_HOST";
        private const string NatsUrl = "NATS_URL";

        string IConfigurationProvider.RedisHost() => GetFromEnv(RedisHost);

        string IConfigurationProvider.NatsUrl() => GetFromEnv(NatsUrl);
        

        private string GetFromEnv(string key)
        {
            return System.Environment.GetEnvironmentVariable(key);
        }
    }
}