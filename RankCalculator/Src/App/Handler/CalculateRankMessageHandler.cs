using System;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Common.Infrastructure.Storage;

namespace RankCalculator.App.Handler
{
    public class CalculateRankMessageHandler
    {
        private readonly IStorage _storage;
        
        private class CalculateRankTaskData
        {
            public CalculateRankTaskData(string text, string saveRankId)
            {
                Text = text;
                SaveRankId = saveRankId;
            }

            public string Text { get; }
            
            public string SaveRankId { get; }

            public bool Handled { get; set; } = false;
        }

        public CalculateRankMessageHandler(IStorage storage)
        {
            _storage = storage;
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
            
            _storage.Save(taskData.SaveRankId, CalculateRank(taskData.Text).ToString());
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