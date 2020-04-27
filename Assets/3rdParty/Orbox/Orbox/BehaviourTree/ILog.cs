namespace Orbox.BehaviourTree
{
    public interface ILog
    {
        Logger RootCaller { get; set; }

        void Add(Logger logger, Status result);
        void OnRootExit(Logger caller);
    }
}
