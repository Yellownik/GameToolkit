using UnityEngine;
using System.Collections.Generic;

namespace Orbox.Utils
{
    public class LoadBalancer : MonoBehaviour, ILoadBalancer
    {
        private LinkedList<ILoadBalanced> Items = new LinkedList<ILoadBalanced>();
        private int BalanceLimit = 5;

        // --- unity ---
        private void Update()
        {
            int count = (Items.Count > BalanceLimit) ? BalanceLimit : Items.Count;
            var node = Items.First;

            for (int i = 0; i < count; i++)
            {
                node.Value.Execute();

                var next = node.Next;
                Items.RemoveFirst();
                Items.AddLast(node);

                node = next;
            }
        }

        public void SetLimit(int count)
        {
            BalanceLimit = count;
        }

        public LinkedListNode<ILoadBalanced> Add(ILoadBalanced item)
        {
            var node = new LinkedListNode<ILoadBalanced>(item);
            Items.AddLast(node);

            return node;
        }

        public void Remove(LinkedListNode<ILoadBalanced> node)
        {
            Items.Remove(node);
        }


    }
}