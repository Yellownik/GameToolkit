using UnityEngine;
using Orbox.Utils;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Orbox.Localization
{
    public class LocalizationManager : ILocalizationManager
    {
        private struct Key
        {
            public readonly Type Type;
            public readonly string Name;
            public readonly string Language;

            public Key(Type type, string name, string language)
            {
                Type = type;
                Name = name;
                Language = language;
            }
        }

        private class Comparer : IEqualityComparer<Key>
        {
            public bool Equals(Key x, Key y)
            {
                return x.Type == y.Type && x.Name == y.Name && x.Language == y.Language;
            }

            public int GetHashCode(Key key)
            {
                return key.Type.GetHashCode() ^ key.Name.GetHashCode() ^ key.Language.GetHashCode();
            }
        }

        private IEnumCache Cache;
        private ILanguageProvider LanguageProvider;
        private Dictionary<Key, string> Items = new Dictionary<Key, string>(new Comparer());

        public LocalizationManager(IEnumCache cache, ILanguageProvider languageProvider)
        {
            Cache = cache;
            LanguageProvider = languageProvider;
        }

        public void Add<TEnum>(LocalizationModule<TEnum> module) 
            where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            foreach(var item in module)
            {
                var key = GetComparerKey(item.Key, module.Language);
                Items[key] = item.Value;
            }
        }

        public string Get<TEnum>(TEnum localizationKey, SystemLanguage language)
            where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            var key = GetComparerKey(localizationKey, language);
            return Items[key];
        }

        public string GetSystem<TEnum>(TEnum localizationKey)
            where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            var language = Application.systemLanguage;
            var value = Get(localizationKey, language);

            return value;
        }

        public string GetNative<TEnum>(TEnum localizationKey)
            where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            var language = LanguageProvider.GetLanguage();
            var value = Get(localizationKey, language);

            return value;
        }

        private Key GetComparerKey<TEnum>(TEnum localizationKey, SystemLanguage language) 
            where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            var type = typeof(TEnum);
            var name = Cache.GetName(localizationKey);
            var languageString = Cache.GetName(language);

            var key = new Key(type, name, languageString);
            return key;
        }
    }
}