using System;
using System.Collections;
using System.Collections.Generic;

namespace Orbox.BehaviourTree
{
    public partial class Debugger
    {
        public static void AddLogging(ref Node root, ref Pointer pointer, ILog log)
        {
            var running = pointer.Node;

            pointer = new LoggerPointer(log);
            Behave.InjectPointer(root, pointer); 

            pointer.Node = running;

            root = AttachLog(root, log); 
        }


        private static Logger AttachLog(Node node, ILog log)
        {
            //int id = log.CreateID();
            var wrapper = new Logger(log, node);

            if (node is Branch == false)
                return wrapper;

            var list = new List<Node>();
            var branch = node as Branch;


            foreach (var child in branch) //save children
                list.Add(child);


            foreach (var child in list) //clear branch
                branch.Remove(child);


            foreach (var child in list) //add wrapped children
            {
                var childWrapper = AttachLog(child, log);
                branch.Add(childWrapper);

                Logger.AssignParent(child, wrapper); //override parent
            }

            return wrapper;
        }


    }
}