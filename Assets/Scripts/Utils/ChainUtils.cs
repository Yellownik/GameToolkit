using System;

public static class ChainUtils
{
    public static TOut As<TIn, TOut>(this TIn self, Func<TIn, TOut> map)
    {
        return map(self);
    }

    public static TOut Using<TIn, TOut>(this TIn self, Func<TIn, TOut> map) where TIn : IDisposable
    {
        var result = map(self);
        self.Dispose();
        return result;
    }

    public static T Do<T>(this T self, Action<T> set)
    {
        set.Invoke(self);
        return self;
    }

    public static T Do<T>(this T self, Action<T> apply, bool when)
    {
        if (when)
            apply.Invoke(self);

        return self;
    }

    public static T Do<T>(this T self, Action<T> apply, Func<bool> when)
    {
        if (when())
            apply.Invoke(self);

        return self;
    }
}
