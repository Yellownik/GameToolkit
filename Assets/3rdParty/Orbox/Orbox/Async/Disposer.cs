using System;
using System.Collections.Generic;
using Orbox.Collections;

namespace Orbox.Async
{

    public class Disposer : IDisposer
    {
        protected List<Action> DisposeActions = new ZeroList<Action>();

        public void Add(Action onDispose)
        {
            DisposeActions.Add(onDispose);
        }

        public void Dispose()
        {
            foreach (var action in DisposeActions)
                action();
        }
    }
}