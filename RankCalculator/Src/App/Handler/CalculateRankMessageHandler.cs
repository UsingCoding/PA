using System;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Common.App.Event;
using Common.Infrastructure.Storage;
using RankCalculator.App.Event;

namespace RankCalculator.App.Handler
{
    public class CalculateRankMessageHandler
    {
        private readonly IStorage _storage;
        private readonly IEventDispatcher _eventDispatcher;
        
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

        public CalculateRankMessageHandler(IStorage storage, IEventDispatcher eventDispatcher)
        {
            _storage = storage;
            _eventDispatcher = eventDispatcher;
        }

        public void Handle(byte[] data)
        {
            var taskId = Encoding.UTF8.GetString(data);
                 
            Console.WriteLine("Processing task - {0}", taskId);

            var serializedTaskData = _storage.Get(taskId);
            
            var taskData = JsonSerializer.Deserialize<CalculateRankTaskData>(serializedTaskData);

            if (taskData == null)
            {
                Console.WriteLine("No task data found for - {0}", taskId);
                return;
            }

            if (taskData.Handled)
            {
                Console.WriteLine("Task - {0} - already handled", taskId);
                return;
            }

            taskData.Handled = true;

            serializedTaskData = JsonSerializer.Serialize(taskData);
            _storage.Save(taskId, serializedTaskData);

            var text = _storage.Get(taskData.TextId);

            if (text == null)
            {
                Console.WriteLine("No text found by - " + taskData.TextId);
                return;
            }

            var rank = CalculateRank(text);
            _storage.Save(taskData.SaveRankId, rank.ToString());
            
            PublishEvent(taskData.TextId, rank);
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