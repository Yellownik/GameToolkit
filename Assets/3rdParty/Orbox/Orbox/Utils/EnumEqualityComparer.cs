using UnityEngine;
using System;
using System.Collections.Generic;



//TODO: move to Orbox.Collections
namespace Orbox.Utils
{
    public struct EnumComparerKey
    {
        public readonly Type Type;
        public readonly int  Value;

        public EnumComparerKey(Type type, int value)
        {
            Type = type;
            Value = value;
        }
    }

    public class EnumEqualityComparer : IEqualityComparer<EnumComparerKey>
    {
        public bool Equals(EnumComparerKey x, EnumComparerKey y)
        {
            return x.Type == y.Type && x.Value == y.Value;
        }

        public int GetHashCode(EnumComparerKey key)
        {
            return key.Type.GetHashCode() ^ key.Value.GetHashCode();
        }
    }
}