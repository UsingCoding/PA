namespace Common.App.Event
{
    public interface IEventSource
    {
        public interface ISubscriber
        {
            void Handle(Event e);
        }
        
        public ISubscription Subscribe(string eventType, ISubscriber subscriber);
        
        public interface ISubscription
        {
            public void Unsubscribe();
        }
    }
}