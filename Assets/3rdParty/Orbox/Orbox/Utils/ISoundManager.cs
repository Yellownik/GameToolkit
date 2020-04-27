using System;
using UnityEngine;

using Orbox.Async;

namespace Orbox.Utils
{
    public interface ISoundManager
    {
        void Play<TEnum>(TEnum sound) where TEnum : struct, IComparable, IConvertible, IFormattable;
        IPromise PlayAndNotify<TEnum>(TEnum sound) where TEnum : struct, IComparable, IConvertible, IFormattable;

        void Pause();
        void Resume();
        
        void Stop();
        void Stop<TEnum>();
        void Stop<TEnum>(TEnum sound) where TEnum : struct, IComparable, IConvertible, IFormattable;

        void SetMute(bool mute);
        void SetMute<TEnum>(bool mute);
        void SetMute<TEnum>(TEnum sound, bool mute) where TEnum : struct, IComparable, IConvertible, IFormattable;

        void SetVolume(float volume);
        void SetVolume<TEnum>(float volume);
        void SetVolume<TEnum>(TEnum sound, float volume) where TEnum : struct, IComparable, IConvertible, IFormattable;

        void SetPitch(float pitch);
        void SetPitch<TEnum>(float pitch);
        void SetPitch<TEnum>(TEnum sound, float pitch) where TEnum : struct, IComparable, IConvertible, IFormattable;

        bool GetMute();
        bool GetMute<TEnum>();
        bool GetMute<TEnum>(TEnum sound) where TEnum : struct, IComparable, IConvertible, IFormattable;

        float GetVolume();
        float GetVolume<TEnum>();
        float GetVolume<TEnum>(TEnum sound) where TEnum : struct, IComparable, IConvertible, IFormattable;

        float GetPitch();
        float GetPitch<TEnum>();
        float GetPitch<TEnum>(TEnum sound) where TEnum : struct, IComparable, IConvertible, IFormattable;

        // To manage the sounds which were created outside of SoundManager
        void Register<TEnum>(TEnum sound, AudioSource source) where TEnum : struct, IComparable, IConvertible, IFormattable;
    }

}