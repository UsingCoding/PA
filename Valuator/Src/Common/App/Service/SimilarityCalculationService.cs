using System.Text;
using System.Text.Json;
using Valuator.Common.App.Event;
using Valuator.Infrastructure.Storage;

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
                this.textId = textId;
                this.isSimilar = isSimilar;
            }

            public string textId;
            public bool isSimilar;
        }

        public SimilarityCalculationService(IStorage storage, IEventDispatcher eventDispatcher)
        {
            _storage = storage;
            _eventDispatcher = eventDispatcher;
        }

        public bool CalculateSimilarity(string text, string id)
        {
            var texts = _storage.GetAllTexts();
            var isSimilar = texts.Exists(value => value == text);
            
            var similarityKey = "SIMILARITY-" + id;
            _storage.Save(similarityKey, isSimilar ? "1" : "0");

            var serializedEventPayload = JsonSerializer.Serialize(new EventPayload(id, isSimilar));

            var e = new IEventDispatcher.Event(SimilarityCalculatedEventType, Encoding.UTF8.GetBytes(serializedEventPayload));
            
            _eventDispatcher.Dispatch(e);
            
            return isSimilar;
        }
    }
}