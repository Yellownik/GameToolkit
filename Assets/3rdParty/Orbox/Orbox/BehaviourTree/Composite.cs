using System;
using System.Collections;
using System.Collections.Generic;

namespace Orbox.BehaviourTree
{
    public abstract class Composite : Branch
    {
        public string Name;
        protected List<Node> Children = new List<Node>();

        public Composite()
        {
        }

        public Composite(string name)
        {
            Name = name;
        }

        public override Node Add(Node node)
        {
            Children.Add(node);
            base.Add(node);
            
            return node; //todo: Убрать 
        }

        public override void Remove(Node node)
        {
            Children.Remove(node);
            base.Remove(node);                        
        }

        public override IEnumerator<Node> GetEnumerator()
        {
            return Children.GetEnumerator();
        }

        public override void Reset()
        {
            for(int i = 0; i < Children.Count; i++)
            {
                Children[i].Reset();
            }
        }
    }
}