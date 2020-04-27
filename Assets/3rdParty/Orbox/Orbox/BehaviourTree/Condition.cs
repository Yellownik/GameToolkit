using System;

namespace Orbox.BehaviourTree
{
    public class Condition : Leaf
    {
        private Predicate<object> Check; 


        public Condition(Predicate<object> condition)
        {
            Check = condition;
        }

        public override Status Process()
        {
            if(Check(null) == true)
                return Status.Success;

            return Status.Failure;
        }
    }
}