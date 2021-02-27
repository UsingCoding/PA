namespace Valuator.Common.App.Configuration
{
    public interface IConfigurationProvider
    {
        public string RedisHost();
        public string NatsUrl();
    }
}