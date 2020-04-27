using System;
using UnityEngine;

namespace Orbox.Localization
{
    public interface ILocalizationManager
    {
        void Add<TEnum>(LocalizationModule<TEnum> module) 
            where TEnum : struct, IComparable, IConvertible, IFormattable;

        string Get<TEnum>(TEnum localizationKey, SystemLanguage language) 
            where TEnum : struct, IComparable, IConvertible, IFormattable;

        string GetSystem<TEnum>(TEnum localizationKey)
            where TEnum : struct, IComparable, IConvertible, IFormattable;

        string GetNative<TEnum>(TEnum localizationKey)
            where TEnum : struct, IComparable, IConvertible, IFormattable;

    }
}