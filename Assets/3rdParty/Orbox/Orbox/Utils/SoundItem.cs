using System;
using UnityEngine;

using Orbox.Signals;
using Orbox.Async;

namespace Orbox.Utils
{
    public partial class SoundManager
    {
        private class SoundItem : IDisposable, IUpdatable
        {
            public bool Free;

            public readonly Type EnumType;
            public readonly int EnumValue;

            public readonly bool External;
            public readonly AudioSource Source;

            private readonly bool OriginalMute;
            private readonly float OriginalPitch;
            private readonly float OriginalVolume;

            private Promise DeferredComplete;

            public SoundItem(Type enumType, int enumValue, AudioSource source, bool external)
            {
                EnumType = enumType;
                EnumValue = enumValue;

                Source = source;
                External = external;

                OriginalMute = source.mute;
                OriginalPitch = source.pitch;
                OriginalVolume = source.volume;

                Free = true;
            }
            
            public void Play()
            {
                Source.Play();
            }

            public IPromise PlayAndNotify()
            {
                DeferredComplete = new Promise();
                Source.Play();

                return DeferredComplete;
            }

            public void Pause()
            {
                Source.Pause();
            }

            public void Resume()
            {
                Source.UnPause();
            }

            public void Stop()
            {
                DeferredComplete?.Resolve();
                Dispose();
            }
                       
            public void Dispose()
            {
                Source.Stop();

                Source.time = 0;

                Source.mute = OriginalMute;
                Source.pitch = OriginalPitch;
                Source.volume = OriginalVolume;

                DeferredComplete = null;
                Free = true;
            }

            public void SetSetting(SoundSetting setting)
            {
                Source.mute = setting.Mute;
                Source.pitch = OriginalPitch * setting.Pitch;
                Source.volume = OriginalVolume * setting.Volume;
            }

            public void Update()
            {
                if (Free == false && 
                    Source.loop == false &&
                    Source.isPlaying == false &&
                    (Mathf.Approximately(Source.clip.length - Source.time, 0f) ||
                    Mathf.Approximately(Source.time, 0f)))
                {
                    DeferredComplete?.Resolve();
                    Dispose();
                }
            }
        }

    }
}