using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Orbox.BehaviourTree
{

    public class LogUnity: ILog
    {
        public Logger RootCaller { get; set; }


        private int IDCounter;

        private List<Item> Current = new List<Item>();
        private List<Item> Previous = new List<Item>();

        

        private class Item : IEquatable<Item>
        {
            public readonly Logger Logger;
            public readonly Status Result;

            public Item(Logger logger, Status result)            
            {
                Logger = logger;
                Result = result;
            }

            public bool Equals(Item other)
            {
                return Logger.Equals(other.Logger) && Result == other.Result;
            }
        }

        public LogUnity()
        {

        }

        public int CreateID()
        {
            return ++IDCounter;
        }

        public void Add(Logger logger, Status result)
        {
            var item = new Item(logger, result);
            Current.Add(item);
        }

        public void OnRootExit(Logger caller)
        {
            if (Current.SequenceEqual(Previous) == false)
            {
                var msg = Prepare("", caller, 0);
                Debug.Log(msg);
            }

            Previous = Current;
            Current = new List<Item>();
        }


        //--- private ---
        private string Prepare(string msg, Logger logger, int depth)
        {
            msg += Format(logger, depth);

            if (logger.Node is Branch == false)
                return msg;

            depth += GetSpaceCount(logger);

            foreach (Logger child in logger.Node as Branch)
                msg = Prepare(msg, child, depth);

            return msg;
        }

        private static int GetSpaceCount(Logger logger)
        {
            int lenght = logger.Node.GetType().Name.Length;
            lenght += (int)(lenght * 0.4f);

            return lenght;
        }

        private string Format(Logger logger, int depth)
        {
            string indent = "";

            if (logger.Parent is Composite)
            {
                indent += System.Environment.NewLine;
                indent += new String(' ', depth);
            }

            var type = logger.Node.GetType().Name;
            var name = logger.Node is Composite ? (logger.Node as Composite).Name: "";            

            var item = Current.Find(i => i.Logger == logger);

            if (item != null)
            {
                switch (item.Result)
                {
                    case Status.Failure: type = "<color=brown>" + type + "</color>"; break;
                    case Status.Running: type = "<color=teal>" + type + "</color>"; break;
                    case Status.Success: type = "<color=green>" + type + "</color>"; break;
                }
            }

            var format = "{0}{1}";

            if (logger.Node is Composite) format = "{0}{1} '{2}'";
            if (logger.Parent is Decorator) format = "{0} -> {1}";

            var msg = String.Format(format, indent, type, name);
            return msg;
        }
    }

}