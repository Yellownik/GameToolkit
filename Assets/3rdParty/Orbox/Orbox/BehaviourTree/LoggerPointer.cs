using System.Linq;

namespace Orbox.BehaviourTree
{
    public class LoggerPointer : Pointer 
    {
       // ILog Log; //
        Node FNode;

        public LoggerPointer(ILog log)
        {
            //Log = log;
        }

        public override Node Node
        {
            get
            {
                return FNode;
            }
            set
            {
                FNode = GetLoggerByNode(value);//new Logger(Log, value);
            }

        }

        private Logger GetLoggerByNode(Node node)
        {
            if (node == null)
                return null;

            var parent = (node.Parent as Logger).Node;            
            var branch = parent as Branch;

            var logger = branch.Where( child => (child as Logger).Node == node).Single();

            return logger as Logger;
        }
    }
}