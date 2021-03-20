namespace EventsLogger.App.Logger
{
    public interface ILogger<TMessage>
    {
        public void Info(TMessage msg);
        public void Error(TMessage msg);
        public void Debug(TMessage msg);
    }
}