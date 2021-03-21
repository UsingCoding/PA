using Common.Infrastructure.Event.MessageBroker;

namespace Valuator.Infrastructure.Event
{
    public class EventChannelResolver : EventDispatcher.IEventChannelResolver
    {
        private const string ChannelName = "valuator.event.similarity_calculated";
        public string ResolveChannel(string eventType)
        {
            return ChannelName;
        }
    }
}