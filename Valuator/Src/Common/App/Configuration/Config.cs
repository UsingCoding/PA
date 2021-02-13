namespace Valuator.Common.App.Configuration
{
    public class Config : IConfigurationProvider
    {
        private const string RedisHost = "REDIS_HOST";

        string IConfigurationProvider.RedisHost() => GetFromEnv(RedisHost);

        private string GetFromEnv(string key)
        {
            return System.Environment.GetEnvironmentVariable(key);
        }
    }
}