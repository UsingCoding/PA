using Common.App.Event;
using Common.Infrastructure.MessageBroker;

namespace Common.Infrastructure.Event.MessageBroker
{
    public class EventSubscription : IEventSource.ISubscription
    {
        private readonly IMessageBroker.ISubscription _subscription;

        public EventSubscription(IMessageBroker.ISubscription subscription)
        {
            _subscription = subscription;
        }

        public void Deconstruct(out IMessageBroker.ISubscription subscription)
        {
            subscription = _subscription;
            subscription.Unsubscribe();
        }

        public void Unsubscribe()
        {
            _subscription.Unsubscribe();
        }
    }
}