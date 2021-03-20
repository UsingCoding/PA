namespace Common.App.Event
{
    public interface IEventDispatcher
    {
        
        public void Dispatch(Event e);
    }
}