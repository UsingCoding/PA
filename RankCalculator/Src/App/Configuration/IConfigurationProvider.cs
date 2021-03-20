namespace RankCalculator.App.Configuration
{
    public interface IConfigurationProvider
    {
        public string RedisHost();
        public string NatsUrl();
        public string ProcessingRankChannel();
        public string RankCalculatorQueue();
    }
}