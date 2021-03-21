namespace Common.Infrastructure.MessageBroker
{
    public interface IMessageBroker
    {
        public void Publish(string channel, byte[] data);
        
        public interface ISubscriber
        {
            void Handle(byte[] data);
        }
        
        public ISubscription Subscribe(string channel, ISubscriber subscriber);
        public ISubscription Subscribe(string channel, string queueName, ISubscriber subscriber);
        
        public interface ISubscription
        {
            public void Unsubscribe();
        }
    }
}