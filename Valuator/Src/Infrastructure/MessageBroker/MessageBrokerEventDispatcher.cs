using System.Text;
using System.Text.Json;
using Valuator.Common.App.Event;

namespace Valuator.Infrastructure.MessageBroker
{
    public class MessageBrokerEventDispatcher : IEventDispatcher
    {
        private readonly IMessageBroker _messageBroker;

        private const string EventsChannel = "valuator.event";

        public MessageBrokerEventDispatcher(IMessageBroker messageBroker)
        {
            _messageBroker = messageBroker;
        }

        public void Dispatch(IEventDispatcher.Event e)
        {
            _messageBroker.Publish(EventsChannel, Encoding.UTF8.GetBytes(JsonSerializer.Serialize(e)));
        }
    }
}