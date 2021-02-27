namespace Valuator.Infrastructure.MessageBroker
{
    public interface IMessageBroker
    {
        public void Publish(string channel, byte[] data);
    }
}