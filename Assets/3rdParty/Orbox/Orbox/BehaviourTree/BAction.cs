using System;

namespace Orbox.BehaviourTree
{   
    public class BAction : Leaf
    {
        private Action Action;

        public BAction(Action process)
        {
            Action = process;
        }

        public override Status Process()
        {
            Action();
            return Status.Success;
        }
    }
}
