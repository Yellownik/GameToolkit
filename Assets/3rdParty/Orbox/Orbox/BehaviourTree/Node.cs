namespace Orbox.BehaviourTree
{

    public abstract class Node 
    {        
        public Branch Parent { get; protected set; }

        public abstract Status Process();
        public abstract void Reset();

        protected static void AssignParent(Node node, Branch parent)
        {
            node.Parent = parent;
        }
    }
}
