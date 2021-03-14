namespace EventsLogger.App.Configuration
{
    public interface IConfigurationProvider
    {
        public string NatsUrl();
    }
}