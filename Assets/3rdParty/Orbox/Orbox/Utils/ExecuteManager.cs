using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace Orbox.Utils
{
    public class ExecuteManager : IExecuteManager
    {
        private List<Action> LastAwakeActions = new List<Action>();

        public void RaiseLastAwake()
        {
            foreach (var action in LastAwakeActions)
                action();

            LastAwakeActions.Clear();
        }

        public void RunOnLastAwake(Action action)
        {
            LastAwakeActions.Add(action);
        }
    }
}