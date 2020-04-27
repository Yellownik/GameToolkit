using System.Collections;
using System.Collections.Generic;

namespace Orbox.BehaviourTree
{
    public abstract class Branch : Node, IEnumerable<Node>
    {
        public abstract Status ProcessChildResult(Status status);
        public abstract IEnumerator<Node> GetEnumerator();

        public virtual Node Add(Node node)
        {
            AssignParent(node, this);
            return node;
        }
        
        public virtual void Remove(Node node)
        {
            AssignParent(node, null);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
}