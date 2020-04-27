using System.Collections.Generic;
namespace Orbox.BehaviourTree
{
    public static class Behave
    {

        public static Status Tick(Node root, Pointer pointer, Status last)
        {
            // start
            if (last != Status.Running)
                return root.Process();


            // continue
            var parent = pointer.Node.Parent;
            var status = pointer.Node.Process();

            if (status != Status.Running)
                status = parent.ProcessChildResult(status);

            return status;
        }


        public static void InjectPointer(Node node, Pointer pointer)
        {

            if (node is IContinuable)
                (node as IContinuable).SetPointer(pointer);

            if(node is Branch == false)
                return;

            foreach (Node child in node as Branch)                
                InjectPointer(child, pointer); // recursive

        }



 
    }
}