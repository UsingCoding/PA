using System.Text.Json;
using Common.App.Event;
using EventsLogger.App.Event;
using EventsLogger.App.Logger;

namespace EventsLogger.App.Handler
{
    public class LoggingEventHandler : IEventSource.ISubscriber
    {
        public class SimilarityCalculatedEventPayload
        {
            public SimilarityCalculatedEventPayload(string textId, bool isSimilar)
            {
                TextId = textId;
                IsSimilar = isSimilar;
            }

            public string TextId { get; }
            public bool IsSimilar { get; }
        }
        
        public class RankCalculatedEventPayload
        {
            public RankCalculatedEventPayload(string textId, double rank)
            {
                TextId = textId;
                Rank = rank;
            }

            public string TextId { get; }
            public double Rank { get; }
        }

        private readonly ILogger<SimilarityCalculatedEventPayload> _similarityEventLogger;
        private readonly ILogger<RankCalculatedEventPayload> _rankCalculatedEventLogger;
        private readonly IStringLogger _logger;

        public LoggingEventHandler(ILogger<SimilarityCalculatedEventPayload> similarityEventLogger, ILogger<RankCalculatedEventPayload> rankCalculatedEventLogger, IStringLogger logger)
        {
            _similarityEventLogger = similarityEventLogger;
            _rankCalculatedEventLogger = rankCalculatedEventLogger;
            _logger = logger;
        }
        
        public void Handle(Common.App.Event.Event e)
        {
            switch (e.Type)
            {
                case Events.ValuatorSimilarityCalculated:
                {
                    var payload = JsonSerializer.Deserialize<SimilarityCalculatedEventPayload>(e.Payload);
                    _similarityEventLogger.Info(payload);
                    break;
                }
                case Events.RankCalculatorRankCalculated:
                {
                    var payload = JsonSerializer.Deserialize<RankCalculatedEventPayload>(e.Payload);
                    _rankCalculatedEventLogger.Info(payload);
                    break;
                }
                default:
                    _logger.Debug("Unknown event type - " + e.Type);
                    break;
            }
        }
    }
}