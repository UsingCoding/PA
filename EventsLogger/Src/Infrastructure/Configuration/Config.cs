namespace EventsLogger.Infrastructure.Configuration
{
    public class Config : IConfigurationProvider
    {
        private const string NatsUrlKey = "NATS_URL";

        public string NatsUrl() => GetFromEnv(NatsUrlKey);
        

        private static string GetFromEnv(string key)
        {
            return System.Environment.GetEnvironmentVariable(key);
        }
    }
}