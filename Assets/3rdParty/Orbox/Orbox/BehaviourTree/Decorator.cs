using System;
using System.Collections;
using System.Collections.Generic;

namespace Orbox.BehaviourTree
{

    public abstract class Decorator : Branch
    {
        protected Node Child;

        public override Node Add(Node node)
        {
            var current = this;

            while (current.Child != null && current.Child is Decorator)
            {
                current = current.Child as Decorator;
            }

            AssignParent(node, current);
            current.Child = node;

            return this;
        }

        public override void Remove(Node node)
        {
            Child = null;
            base.Remove(node);
        }

        public override void Reset()
        {
            Child.Reset();
        }

        public override IEnumerator<Node> GetEnumerator()
        {
            return Yield().GetEnumerator();
        }

        public static Decorator operator +(Decorator node1, Node node2)
        {
            return node1.Add(node2) as Decorator;
        }

        private IEnumerable<Node> Yield()
        {
            yield return Child;
        }

    }

}