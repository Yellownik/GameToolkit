namespace Orbox.BehaviourTree
{
    //Priority selectors are simply an ordered list of behaviors that are tried one after the other until one matches.

    public class Selector : Composite
    {
        private int Index;

        public Selector()
        {
        }

        public Selector(string name): base(name)
        {
        }


        //TODO: need refactoring
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

                if (result == Status.Success) 
                {
                    ResetSelf();
                    return Status.Success;
                }

                if (result == Status.Running) 
                    return Status.Running;

                Index++;
            }

            ResetSelf();
            return Status.Failure;
        }

        public override Status ProcessChildResult(Status status)
        {           
            if(status == Status.Failure)
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
