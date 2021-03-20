using Common.Infrastructure.MessageBroker;
using NATS.Client;

namespace Common.Infrastructure.Nats.MessageBroker
{
    public class NatsSubscription : IMessageBroker.ISubscription
    {
        private readonly IAsyncSubscription _subscription;

        public NatsSubscription(IAsyncSubscription subscription)
        {
            _subscription = subscription;
        }

        public void Unsubscribe()
        {
            _subscription.Unsubscribe();   
        }

        public void Deconstruct(out IAsyncSubscription subscription)
        {
            subscription = _subscription;
            subscription.Unsubscribe();
        }
    }
}