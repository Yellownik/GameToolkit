namespace Orbox.Signals
{
    public interface IEventPublisher
    {
        void Add(IEventSubscriber subscriber);
    }
}