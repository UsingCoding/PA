using Common.Infrastructure.Event.MessageBroker;

namespace RankCalculator.Infrastructure.Event
{
    public class EventChannelResolver : EventDispatcher.IEventChannelResolver
    {
        private const string ChannelName = "rank_calculator.event.rank_calculated";
        public string ResolveChannel(string eventType)
        {
            return ChannelName;
        }
    }
}