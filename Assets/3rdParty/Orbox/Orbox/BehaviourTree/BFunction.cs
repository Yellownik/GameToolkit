using System;

namespace Orbox.BehaviourTree
{

    public class BFunction : Leaf, IContinuable
    {
        private Pointer Pointer;
        private Func<Status> Function;

        public BFunction(Func<Status> process)
        {
            Function = process;
        }

        public void SetPointer(Pointer pointer)
        {
            Pointer = pointer;
        }

        public override Status Process()
        {
            var result = Function();

            if (result == Status.Running)
            {
                Pointer.Node = this;
            }
                
            return result;
        }
    }
}