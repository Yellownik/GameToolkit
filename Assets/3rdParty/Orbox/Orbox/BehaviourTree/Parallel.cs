using System;

namespace Orbox.BehaviourTree
{
    //Возвращет Success если хотябы один ребенок вернул Success
    //Возвращает Failure если все дети вернули Failure иначе Running
    public class Parallel : Composite, IContinuable
    {
        private Pointer Pointer;

        public Parallel()
        {
        }

        public Parallel(string name): base(name)
        {
        }

        public void SetPointer(Pointer pointer)
        {
            Pointer = pointer;
        }

        public override Status Process()
        {
            bool isAtLeastOnRunning = false;

            for (int i = 0; i < Children.Count; i++)
            {
                var result = Children[i].Process();

                if (result == Status.Success)
                {
                    Reset(); //reset subtree 
                    return Status.Success;
                }

                if (result == Status.Running)
                    isAtLeastOnRunning = true;
            }

            if (isAtLeastOnRunning == true)
            {
                Pointer.Node = this;
                return Status.Running;
            }

            return Status.Failure;
        }

        public override Status ProcessChildResult(Status status)
        {           
            throw new InvalidOperationException("Cannot process child from Parallel node.");
        }




    }
}