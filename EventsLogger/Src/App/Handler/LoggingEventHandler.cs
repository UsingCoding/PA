using System;
using System.Text.Json;
using Common.App.Event;
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

        private readonly ILogger<SimilarityCalculatedEventPayload> _similarityEventLogger;

        public LoggingEventHandler(ILogger<SimilarityCalculatedEventPayload> similarityEventLogger)
        {
            _similarityEventLogger = similarityEventLogger;
        }
        
        public void Handle(Common.App.Event.Event e)
        {
            var payload = JsonSerializer.Deserialize<SimilarityCalculatedEventPayload>(e.Payload);
            _similarityEventLogger.Info(payload);
        }
    }
}