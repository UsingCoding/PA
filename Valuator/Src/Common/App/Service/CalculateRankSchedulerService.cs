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
          private readonly string _text;
          private bool _handled = false;

          public CalculateRankTaskData(string text)
          {
              _text = text;
          }

          public string Text => _text;

          public bool Handled
          {
              get => _handled;
              set => _handled = value;
          }
      }

      private const string TaskKeyPrefix = "CALC-RANK-";
      private const string RankCalculatorChannel = "valuator.processing.rank";

      private readonly IStorage _storage;
      private readonly IMessageBroker _messageBroker;

      public CalculateRankSchedulerService(IStorage storage, IMessageBroker messageBroker)
      {
          _storage = storage;
          _messageBroker = messageBroker;
      }

      public void PostCalculateRankMessage(string text)
      {
          var id = Guid.NewGuid().ToString();

          var taskData = new CalculateRankTaskData(text);

          var serializedTaskData = JsonSerializer.Serialize(taskData);

          var taskId = TaskKeyPrefix + id;
          
          _storage.Save(taskId, serializedTaskData);
          
          _messageBroker.Publish(RankCalculatorChannel, Encoding.UTF8.GetBytes(taskId));
      }
  }
}