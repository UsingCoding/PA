using System;
using Common.Infrastructure.Event.MessageBroker;
using EventsLogger.App.Event;

namespace EventsLogger.Infrastructure.Event
{
    public struct EventsChannels
    {
        public const string ValuatorEventChannel = "valuator.event.similarity_calculated";
        public const string RankCalculatorEventChannel = "rank_calculator.event.rank_calculated";
    }
    
    public class EventChannelResolver : EventDispatcher.IEventChannelResolver
    {
        public string ResolveChannel(string eventType)
        {
            switch (eventType)
            {
                case Events.ValuatorSimilarityCalculated:
                    return EventsChannels.ValuatorEventChannel;
                case Events.RankCalculatorRankCalculated:
                    return EventsChannels.RankCalculatorEventChannel;
            }

            throw new UndefinedEventType();
        }
        
        public class UndefinedEventType : Exception
        {
            
        }
    }
}