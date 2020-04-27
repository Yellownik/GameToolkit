using Orbox.Async;
using System;

namespace Orbox.Utils
{
    public interface IFSM<TEnum> where TEnum : struct, IComparable, IConvertible, IFormattable
    {
        IPromise SetState(TEnum next);
        void AddTransition(TEnum from, TEnum to, Func<IPromise> transiton); 
    }
}