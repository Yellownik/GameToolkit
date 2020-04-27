using System;
using System.Collections.Generic;

namespace Orbox.Utils
{
    //This class do not allocate memory on a heap to obtain cached strings. 
    public class EnumCache : IEnumCache
    {
        private Dictionary<EnumCacheKey, string> Cache = new Dictionary<EnumCacheKey, string>(new EnumCacheComparer());
        private Dictionary<Type, Object> ValuesCache = new Dictionary<Type, Object>();


        public string GetName<T>(T param) where T : struct , IComparable, IConvertible, IFormattable// where T: Enum
        {
            var type = typeof(T);
            int value = EnumInt32ToInt.Convert<T>(param);

            var key = new EnumCacheKey(type, value);

            if (Cache.ContainsKey(key) == false)
            {
                Cache.Add(key, param.ToString());
            }

            var result = Cache[key];
            return result;                                   
        }

        public T[] GetValues<T>() where T : struct, IComparable, IConvertible, IFormattable// where T: Enum
        {
            var key = typeof(T);


            if (ValuesCache.ContainsKey(key) == false)
            {
                var source = Enum.GetValues(key);
                T[] values = new T[source.Length];

                int index = 0;
                foreach (T value in source)
                {
                    values[index] = value;
                    index++;
                }

                ValuesCache.Add(key, values);
            }

            var result = (T[])ValuesCache[key];
            return result;

        }

        private struct EnumCacheKey
        {
            public readonly Type Type;
            public readonly int Value;

            public EnumCacheKey(Type type, int value)
            {
                Type = type;
                Value = value;
            }
        }

        private class EnumCacheComparer : IEqualityComparer<EnumCacheKey>
        {
            public bool Equals(EnumCacheKey x, EnumCacheKey y)
            {
                return x.Type == y.Type && x.Value == y.Value;
            }

            public int GetHashCode(EnumCacheKey key)
            {
                return key.Type.GetHashCode() ^ key.Value.GetHashCode();
            }
        }
        


    }

}