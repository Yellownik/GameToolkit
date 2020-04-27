using System.Collections.Generic;

namespace Orbox.Utils
{
    public interface ILoadBalancer
    {
        void SetLimit(int count);

        LinkedListNode<ILoadBalanced> Add(ILoadBalanced item);
        void Remove(LinkedListNode<ILoadBalanced> node);
    }
}