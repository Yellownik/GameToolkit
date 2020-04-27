using System;
using System.Collections;

namespace Orbox.BehaviourTree
{
    public class Invertor : Decorator
    {
        public override Status Process()
        {
            var result = Child.Process();
            return InvertStatus(result);
        }

        public override Status ProcessChildResult(Status status)
        {
            var result = InvertStatus(status);

            if (Parent != null) // if not Root
                result = Parent.ProcessChildResult(result);

            return result;
        }

        private Status InvertStatus(Status status)
        {
            switch (status)
            {
                case Status.Failure: return Status.Success;
                case Status.Success: return Status.Failure;
                case Status.Running: return Status.Running;

                default: throw new ArgumentOutOfRangeException("Miss Result case");
            }            
        }

    }
}