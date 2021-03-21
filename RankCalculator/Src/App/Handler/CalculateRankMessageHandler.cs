using System;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Common.App.Event;
using Common.App.Logger;
using Common.Infrastructure.Storage;
using RankCalculator.App.Event;

namespace RankCalculator.App.Handler
{
    public class CalculateRankMessageHandler
    {
        private readonly IStorage _storage;
        private readonly IEventDispatcher _eventDispatcher;
        private readonly IStringLogger _logger;
        
        private class CalculateRankTaskData
        {
            public CalculateRankTaskData(string textId, string saveRankId)
            {
                TextId = textId;
                SaveRankId = saveRankId;
            }

            public string TextId { get; }
            public string SaveRankId { get; set; }

            public bool Handled { get; set; } = false;
        }

        public CalculateRankMessageHandler(IStorage storage, IEventDispatcher eventDispatcher, IStringLogger logger)
        {
            _storage = storage;
            _eventDispatcher = eventDispatcher;
            _logger = logger;
        }

        public void Handle(byte[] data)
        {
            var taskId = Encoding.UTF8.GetString(data);

            var serializedTaskData = _storage.Get(taskId);
            
            var taskData = JsonSerializer.Deserialize<CalculateRankTaskData>(serializedTaskData);

            if (taskData == null)
            {
                _logger.Error("No task data found for - " +taskId);
                return;
            }

            if (taskData.Handled)
            {
                _logger.Error("Task - " + taskId + " - already handled");
                return;
            }

            taskData.Handled = true;

            serializedTaskData = JsonSerializer.Serialize(taskData);
            _storage.Save(taskId, serializedTaskData);

            var text = _storage.Get(taskData.TextId);

            if (text == null)
            {
                _logger.Error("No text found by - " + taskData.TextId);
                return;
            }

            var rank = CalculateRank(text);
            _storage.Save(taskData.SaveRankId, rank.ToString());
            
            PublishEvent(taskData.TextId, rank);
            
            _logger.Info("Task processed  - " + taskId);
        }

        private void PublishEvent(string textId, double rank)
        {
            var payload = new RankCalculatedEventPayload(textId, rank);
            
            var e = new Common.App.Event.Event(Events.RankCalculated, JsonSerializer.Serialize(payload));
            
            _eventDispatcher.Dispatch(e);
        }

        private static double CalculateRank(string text)
        {
            var regexp = new Regex(@"[A-Z,a-z,А-Я,а-я]");
            var nonAlphabetCharsCount = 0;

            foreach (var ch in text)
            {
                if (!regexp.IsMatch(ch.ToString()))
                {
                    nonAlphabetCharsCount++;
                }
            }

            if (nonAlphabetCharsCount == 0)
            {
                return 0;
            }

            return Math.Round(nonAlphabetCharsCount / (double) text.Length, 2);
        }
    }
}