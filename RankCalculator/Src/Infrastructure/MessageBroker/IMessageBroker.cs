namespace RankCalculator.Infrastructure.MessageBroker
{
    public interface IMessageBroker
    {
        public void Publish(string channel, byte[] data);
    }
    
    public interface ISubscription
    {
        void Unsubscribe();
    }
}