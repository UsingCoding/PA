using System;
using System.Text;
using System.Text.Json;
using Common.Infrastructure.MessageBroker;
using Common.Infrastructure.Storage;

namespace Valuator.Common.App.Service
{
  public class CalculateRankSchedulerService
  {
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
      
      private struct Message
      {
          public Message(string id, string taskId)
          {
              Id = id;
              TaskId = taskId;
          }

          public string Id { get; }
          public string TaskId { get; }
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

      public void PostCalculateRankMessage(string id, string textId)
      {
          var taskData = new CalculateRankTaskData(textId, SaveRankKeyPrefix + id);

          var serializedTaskData = JsonSerializer.Serialize(taskData);

          var taskId = TaskKeyPrefix + id;
          
          _storage.Save(id, taskId, serializedTaskData);

          var message = JsonSerializer.Serialize(new Message(id, taskId));
          
          _messageBroker.Publish(RankCalculatorChannel, Encoding.UTF8.GetBytes(message));
      }
  }
}