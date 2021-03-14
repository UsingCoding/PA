using System;
using System.Text;
using System.Text.Json;
using Valuator.Infrastructure.MessageBroker;
using Valuator.Infrastructure.Storage;

namespace Valuator.Common.App.Service
{
  public class CalculateRankSchedulerService
  {
      private class CalculateRankTaskData
      {
          public CalculateRankTaskData(string text, string saveRankId)
          {
              Text = text;
              SaveRankId = saveRankId;
          }

          public string Text { get; }
          public string SaveRankId { get; set; }

          public bool Handled { get; set; } = false;
      }

      private const string TaskKeyPrefix = "CALC-RANK-";
      private const string SaveRankKeyPrefix = "RANK-";
      private const string RankCalculatorChannel = "valuator.processing.rank";

      private readonly IStorage _storage;
      private readonly IMessageBroker _messageBroker;

      public CalculateRankSchedulerService(IStorage storage, IMessageBroker messageBroker)
      {
          _storage = storage;
          _messageBroker = messageBroker;
      }

      public void PostCalculateRankMessage(string text, string textId)
      {
          var id = Guid.NewGuid().ToString();

          var taskData = new CalculateRankTaskData(text, SaveRankKeyPrefix + textId);

          var serializedTaskData = JsonSerializer.Serialize(taskData);

          var taskId = TaskKeyPrefix + id;
          
          _storage.Save(taskId, serializedTaskData);
          
          _messageBroker.Publish(RankCalculatorChannel, Encoding.UTF8.GetBytes(taskId));
      }
  }
}