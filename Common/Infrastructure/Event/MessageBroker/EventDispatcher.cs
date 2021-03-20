using System.Text;
using System.Text.Json;
using Common.App.Event;
using Common.Infrastructure.MessageBroker;

namespace Common.Infrastructure.Event.MessageBroker
{
    public class EventDispatcher : IEventDispatcher, IEventSource
    {
        public interface IEventChannelResolver
        {
            public string ResolveChannel(string eventType);
        }

        private readonly IEventChannelResolver _resolver;
        private readonly IMessageBroker _messageBroker;

        public EventDispatcher(IEventChannelResolver resolver, IMessageBroker messageBroker)
        {
            _resolver = resolver;
            _messageBroker = messageBroker;
        }

        public void Dispatch(App.Event.Event e)
        {
            throw new System.NotImplementedException();
        }

        public IEventSource.ISubscription Subscribe(string eventType, IEventSource.ISubscriber subscriber)
        {
            var channel = _resolver.ResolveChannel(eventType);
            var subscription = _messageBroker.Subscribe(channel, new MessageBrokerSubscriberAdapter(subscriber));

            return new EventSubscription(subscription);
        }
        
        private class MessageBrokerSubscriberAdapter : IMessageBroker.ISubscriber
        {
            private readonly IEventSource.ISubscriber _adaptee;

            public MessageBrokerSubscriberAdapter(IEventSource.ISubscriber adaptee)
            {
                _adaptee = adaptee;
            }

            public void Handle(byte[] data)
            {
                var rawData = Encoding.UTF8.GetString(data);
                var unmarshalledEvent = JsonSerializer.Deserialize<App.Event.Event>(rawData);

                if (unmarshalledEvent == null)
                {
                    throw new App.Event.Event.UnmasrhallingFailedException();
                }
                
                _adaptee.Handle(unmarshalledEvent);
            }
        }
    }
}