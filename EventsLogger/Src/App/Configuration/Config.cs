namespace EventsLogger.App.Configuration
{
    public class Config : IConfigurationProvider
    {
        private const string NatsUrl = "NATS_URL";

        string IConfigurationProvider.NatsUrl() => GetFromEnv(NatsUrl);
        

        private static string GetFromEnv(string key)
        {
            return System.Environment.GetEnvironmentVariable(key);
        }
    }
}