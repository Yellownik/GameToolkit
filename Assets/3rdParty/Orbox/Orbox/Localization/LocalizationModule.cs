using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Orbox.Utils;

namespace Orbox.Localization
{
    public abstract class LocalizationModule<TKey> : IEnumerable<KeyValuePair<TKey, string>> 
        where TKey : struct, IComparable, IConvertible, IFormattable
    {
        public SystemLanguage Language { get; private set; }

        private IEnumCache Cache;
        private Dictionary<TKey, string> Items = new Dictionary<TKey, string>();

        public LocalizationModule(SystemLanguage language)
        {
            Language = language;
        }

        public string this[TKey key]
        {
            set
            {                
                Items[key] = value;
            }
        }

        public IEnumerator<KeyValuePair<TKey, string>> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Items.GetEnumerator();
        }
    }
}