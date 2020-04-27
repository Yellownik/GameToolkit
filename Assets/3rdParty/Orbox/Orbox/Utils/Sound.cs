using UnityEngine;
using System;

namespace Orbox.Utils
{
    [Obsolete("Use SoundManager insted")]
    public class Sound : ISound
    {
        private IResourceManager Resources;
        private AudioSource AudioSource;
        private UpdateTimer Timer;

        public Sound(IResourceManager resources)
        {
            Resources = resources;
            AudioSource = MonoExtensions.MakeComponent<AudioSource>();
            Timer = new UpdateTimer(AudioSource.gameObject);

        }

        public void Play<TEnum>(TEnum sound)
           where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            var clip = Resources.LoadAudioClip(sound);
            AudioSource.PlayOneShot(clip);
        }

        public void Play<TEnum>(TEnum sound, Action onComplete)
           where TEnum : struct, IComparable, IConvertible, IFormattable
        {            
            var clip = Resources.LoadAudioClip(sound);
            AudioSource.PlayOneShot(clip);

            Timer.SetOnce(clip.length, onComplete);
        }

        public void Stop()
        {
            AudioSource.Stop();
        }

    }
}