using System;

namespace Orbox.Utils
{
    [Obsolete("Use ISoundManager insted")]
    public interface ISound 
    {
        void Play<TEnum>(TEnum sound)
           where TEnum : struct, IComparable, IConvertible, IFormattable;

        void Play<TEnum>(TEnum sound, Action onComplete)
           where TEnum : struct, IComparable, IConvertible, IFormattable;

        void Stop();

    }
}