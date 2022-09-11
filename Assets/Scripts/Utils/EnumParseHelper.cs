using System;
using System.Collections.Concurrent;

public sealed class EnumHelper<T> where T : Enum
{
    private static readonly ConcurrentDictionary<string, T> Cache = new(StringComparer.OrdinalIgnoreCase);

    public static T Parse(string s)
    {
        return Cache.GetOrAdd(s, k => (T) Enum.Parse(typeof(T), k));
    }
}
