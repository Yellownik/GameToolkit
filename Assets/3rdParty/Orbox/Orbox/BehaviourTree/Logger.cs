using System;
using System.Collections;
using System.Collections.Generic;

namespace Orbox.BehaviourTree
{

    public class Logger : Branch
    {
        public Node Node;
        private ILog Log;
        

        public Logger(ILog log, Node node)
        {
            Log = log;
            Node = node;
        }

        public override Status Process()
        {
            if (Log.RootCaller == null)
                Log.RootCaller = this;

            var result = Node.Process();
            Log.Add(this, result);

            //var parent = Parent as Logger;

            if(Log.RootCaller == this)
            {
                Log.RootCaller = null;
                Log.OnRootExit(this);
            }


            return result;
        }

        public override Status ProcessChildResult(Status status)
        {
            if (Log.RootCaller == null)
                Log.RootCaller = this;

            var branch = (Branch)Node;
            var result = branch.ProcessChildResult(status);
            Log.Add(this, result);

            if (Log.RootCaller == this)
            {
                Log.RootCaller = null;
                Log.OnRootExit(this);
            }

            return result;
        }

        public static new void AssignParent(Node node, Branch parent)
        {
            Node.AssignParent(node, parent);
        }

        public override void Reset()
        {
            Node.Reset();
        }

        public override IEnumerator<Node> GetEnumerator()
        {
            //should not use it
            throw new NotImplementedException();
        }
    }


}