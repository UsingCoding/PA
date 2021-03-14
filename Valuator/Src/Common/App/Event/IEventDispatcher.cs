namespace Valuator.Common.App.Event
{
    public interface IEventDispatcher
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

        public void Dispatch(Event e);
    }
}