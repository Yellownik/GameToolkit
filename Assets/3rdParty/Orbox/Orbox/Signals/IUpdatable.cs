namespace Orbox.Signals
{
    public interface IUpdatable: IEventSubscriber
    {
        void Update();
    }
}