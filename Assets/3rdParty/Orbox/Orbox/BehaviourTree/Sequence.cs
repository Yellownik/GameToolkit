namespace Orbox.BehaviourTree
{
    //If a child behavior succeeds, the sequence continues its execution.
    //If a child behavior fails, then the sequence code backtracks to find other candidate behaviors

    public class Sequence : Composite
    {
        private int Index;

        public Sequence()
        {
        }

        public Sequence(string name): base(name)
        {
        }

        public override void Reset()
        {
            ResetSelf();
            base.Reset();
        }

        public override Status Process()
        {
            while (Index < Children.Count)
            {
                var result = Children[Index].Process();

                if (result == Status.Failure) 
                {
                    ResetSelf();
                    return Status.Failure;
                }

                if (result == Status.Running) 
                    return Status.Running;

                Index++;
            }

            ResetSelf();
            return Status.Success;
        }

        public override Status ProcessChildResult(Status status)
        {
            if (status == Status.Success)
            {
                Index++;
                status = Process();
            }

            if (Parent != null) // if not Root
                status = Parent.ProcessChildResult(status);

            if (status != Status.Running)
                ResetSelf();

            return status;
        }

        private void ResetSelf()
        {
            Index = 0;
        }
    }
}
