namespace Valuator.Common.App.Event
{
    public interface IEventDispatcher
    {
        public class Event
        {
            public Event(string type, string payload)
            {
                Type = type;
                Payload = payload;
            }

            public string Type { get; }
            public string Payload { get; }
        }

        public void Dispatch(Event e);
    }
}