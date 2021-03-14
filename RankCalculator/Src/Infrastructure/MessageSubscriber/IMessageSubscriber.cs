namespace RankCalculator.Infrastructure.MessageSubscriber
{
    public interface IMessageSubscriber
    {
        public delegate void Handler(byte[] data);
        
        public ISubscription Subscribe(string channel, string queueName, Handler handler);
    }
    
    public interface ISubscription
    {
        void Unsubscribe();
    }
}