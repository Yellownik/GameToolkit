using System;
using Orbox.Async;

namespace Orbox.Utils
{
    public interface ITimers
    {
        IPromise Wait(float seconds);
    }
}
