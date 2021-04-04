using System.Text.Json;
using Common.App.Event;
using Common.Infrastructure.Storage;

namespace Valuator.Common.App.Service
{
    public class SimilarityCalculationService
    {
        private readonly IStorage _storage;
        private readonly IEventDispatcher _eventDispatcher;

        private const string SimilarityCalculatedEventType = "event.similarity_calculated"; 
        
        private struct EventPayload
        {
            public EventPayload(string textId, bool isSimilar)
            {
                TextId = textId;
                IsSimilar = isSimilar;
            }

            public string TextId { get; }
            public bool IsSimilar { get; }
        }

        public SimilarityCalculationService(IStorage storage, IEventDispatcher eventDispatcher)
        {
            _storage = storage;
            _eventDispatcher = eventDispatcher;
        }

        public void CalculateSimilarity(string text, string id, string textId)
        {
            var isSimilar = _storage.IsTextExists(text);
            
            var similarityKey = "SIMILARITY-" + id;
            _storage.Save(id, similarityKey, isSimilar ? "1" : "0");

            var serializedEventPayload = JsonSerializer.Serialize(new EventPayload(textId, isSimilar));

            var e = new Event(SimilarityCalculatedEventType, serializedEventPayload);
            
            _eventDispatcher.Dispatch(e);
        }
    }
}