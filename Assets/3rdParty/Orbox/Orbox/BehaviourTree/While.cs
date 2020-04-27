namespace Orbox.BehaviourTree
{
    public class While:  Decorator, IContinuable
    {
        private Pointer Pointer;
  
        public void SetPointer(Pointer pointer)
        {
            Pointer = pointer;
        }

        public override Status Process()
        {
            var result = Child.Process();

            if (result == Status.Failure)
                return Status.Success;


            if (result == Status.Success)
                Pointer.Node = this;


            return Status.Running;
        }

        public override Status ProcessChildResult(Status status)
        {

            if (status == Status.Success)
            {
                Pointer.Node = this;
                status = Status.Running;
            }
            
            if (status == Status.Failure)
                status = Status.Success;


            if (Parent != null) // if not Root
                status = Parent.ProcessChildResult(status);

            return status;
        }

    }
}