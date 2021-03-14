namespace EventsLogger.Infrastructure.EventSource
{
    public interface IEventSource
    {
        public class Event
        {
            public Event(string type, byte[] payload)
            {
                Type = type;
                Payload = payload;
            }

            public string Type { get; }
            public byte[] Payload { get; }
        }
        
        public interface ISubscriber
        {
            void Handle(Event e);
        }
        
        public ISubscription Subscribe(ISubscriber subscriber);
        
        public interface ISubscription
        {
            public void Unsubscribe();
        }
    }
}