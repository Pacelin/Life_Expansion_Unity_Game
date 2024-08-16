
namespace UniOwl.Events
{
    public interface IEventListener
    {
        
    }

    public interface IEventListener<in T> : IEventListener where T : struct, IEvent
    {
        void OnEvent(T evt);
    }
}