namespace Valuator.Common.App.Configuration
{
    public class Config : IConfigurationProvider
    {
        private string _redisHost = "dotnet_app_redis";
        
        string IConfigurationProvider.RedisHost() => _redisHost;
    }
}