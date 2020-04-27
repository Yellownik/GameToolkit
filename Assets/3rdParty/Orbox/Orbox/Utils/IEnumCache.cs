using UnityEngine;
using System;
using System.Collections;

namespace Orbox.Utils
{
    public interface IEnumCache
    {
        string GetName<T>(T param) where T : struct , IComparable, IConvertible, IFormattable;
        T[] GetValues<T>() where T : struct, IComparable, IConvertible, IFormattable; // where T: Enum
    }
}