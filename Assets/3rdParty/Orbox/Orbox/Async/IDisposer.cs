using System;

namespace Orbox.Async
{

    public interface IDisposer
    {
        void Add(Action onDispose);
        void Dispose();
    }
}