using System;
using System.Collections.Generic;
using Orbox.Async;
using Orbox.Collections;
using Orbox.Signals;
using UnityEngine;


namespace Orbox.Utils
{
    public partial class SoundManager : ISoundManager, IUpdatable
    {
        private struct SoundSetting
        {
            public static SoundSetting Default => new SoundSetting { Volume = 1.0f, Mute = false, Pitch = 1.0f };

            public float Volume;    // 0..1
            public float Pitch;

            public bool Mute;       // true/false
        }

        private List<SoundItem> AudioItems = new List<SoundItem>();
        private ListPool<SoundItem> ListPool = new ListPool<SoundItem>();

        private SoundSetting GlobalSettings = SoundSetting.Default;
        private Dictionary<Type, SoundSetting> GroupSettings = new Dictionary<Type, SoundSetting>();
        private Dictionary<EnumComparerKey, SoundSetting> SoundSettings = new Dictionary<EnumComparerKey, SoundSetting>(new EnumEqualityComparer());

        private IEventPublisher EventPublisher;
        private IResourceManager ResourceManager;
        private Transform Transform;

        public SoundManager(IResourceManager resourceManager, IEventPublisher publisher, Transform transform)
        {
            ResourceManager = resourceManager;
            EventPublisher = publisher;
            Transform = transform;

            publisher.Add(this);
        }

        private SoundItem GetFreeItem<TEnum>(TEnum sound) where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            var type = typeof(TEnum);
            var value = EnumInt32ToInt.Convert(sound);

            for (int i = 0; i < AudioItems.Count; i++)
            {
                var item = AudioItems[i];

                if (item.EnumType == type &&
                    item.EnumValue == value &&
                    item.Free)
                {
                    item.Free = false;
                    return item;
                }
            }

            //create new Item
            var source = ResourceManager.CreatePrefabInstance<TEnum, AudioSource>(sound, Transform);

            var newItem = new SoundItem(type, value, source, false);
            AudioItems.Add(newItem);

            newItem.Free = false;

            return newItem;
        }

        private PoolableList<SoundItem> GetPlayingAudioItemsCopy()
        {
            var list = ListPool.Rent();

            for (int i = 0; i < AudioItems.Count; i++)
            {
                if (AudioItems[i].Free == false)
                    list.Add(AudioItems[i]);
            }

            return list;
        }

        private PoolableList<SoundItem> GetPlayingAudioItemsCopy<TEnum>()
        {
            var type = typeof(TEnum);
            var list = ListPool.Rent();

            for (int i = 0; i < AudioItems.Count; i++)
            {
                if (AudioItems[i].Free == false && AudioItems[i].EnumType == type)
                    list.Add(AudioItems[i]);
            }

            return list;
        }

        private PoolableList<SoundItem> GetPlayingAudioItemsCopy<TEnum>(TEnum sound) where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            var type = typeof(TEnum);
            var value = EnumInt32ToInt.Convert(sound);
            var list = ListPool.Rent();

            for (int i = 0; i < AudioItems.Count; i++)
            {
                if (AudioItems[i].Free == false &&
                    AudioItems[i].EnumType == type &&
                    AudioItems[i].EnumValue == value)
                    list.Add(AudioItems[i]);
            }

            return list;
        }

        private PoolableList<SoundItem> GetInternalAudioItemsCopy<TEnum>(TEnum sound) where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            var type = typeof(TEnum);
            var value = EnumInt32ToInt.Convert(sound);
            var list = ListPool.Rent();

            for (int i = 0; i < AudioItems.Count; i++)
            {
                if (AudioItems[i].External == false &&
                    AudioItems[i].EnumType == type &&
                    AudioItems[i].EnumValue == value)
                    list.Add(AudioItems[i]);
            }

            return list;
        }


        public void Play<TEnum>(TEnum sound)
            where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            var type = typeof(TEnum);
            var value = EnumInt32ToInt.Convert(sound);

            var setting = GetResultingSetting(type, value);

            var item = GetFreeItem(sound);
            item.SetSetting(setting);

            item.Play();
        }

        public IPromise PlayAndNotify<TEnum>(TEnum sound) where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            var type = typeof(TEnum);
            var value = EnumInt32ToInt.Convert(sound);

            var setting = GetResultingSetting(type, value);

            var item = GetFreeItem(sound);
            item.SetSetting(setting);

            return item.PlayAndNotify();
        }

        public void Pause()
        {
            using (var list = GetPlayingAudioItemsCopy())
            {
                foreach (var item in list)
                {
                    item.Pause();
                }
            }
        }

        public void Resume()
        {
            using (var list = GetPlayingAudioItemsCopy())
            {
                foreach (var item in list)
                {
                    item.Resume();
                }
            }
        }

        public void Stop()
        {
            using (var list = GetPlayingAudioItemsCopy())
            {
                foreach (var item in list)
                {
                    item.Stop();
                }
            }
        }

        public void Stop<TEnum>()
        {
            using (var list = GetPlayingAudioItemsCopy<TEnum>())
            {
                foreach (var item in list)
                {
                    item.Stop();
                }
            }
        }

        public void Stop<TEnum>(TEnum sound) where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            using (var list = GetPlayingAudioItemsCopy(sound))
            {
                foreach (var item in list)
                {
                    item.Stop();
                }
            }
        }

        public void SetVolume(float volume)
        {
            var setting = GetSettings();
            setting.Volume = volume;

            SetSettings(setting);
        }

        public void SetVolume<TEnum>(float volume)
        {
            var setting = GetSettings<TEnum>();
            setting.Volume = volume;

            SetSettings<TEnum>(setting);
        }

        public void SetVolume<TEnum>(TEnum sound, float volume)
            where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            var setting = GetSettings(sound);
            setting.Volume = volume;

            SetSettings(sound, setting);
        }

        public void SetMute(bool mute)
        {
            var setting = GetSettings();
            setting.Mute = mute;

            SetSettings(setting);
        }

        public void SetMute<TEnum>(bool mute)
        {
            var setting = GetSettings<TEnum>();
            setting.Mute = mute;

            SetSettings<TEnum>(setting);
        }

        public void SetMute<TEnum>(TEnum sound, bool mute)
            where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            var setting = GetSettings(sound);
            setting.Mute = mute;

            SetSettings(sound, setting);
        }

        public void SetPitch(float pitch)
        {
            var setting = GetSettings();
            setting.Pitch = pitch;

            SetSettings(setting);
        }

        public void SetPitch<TEnum>(float pitch)
        {
            var setting = GetSettings<TEnum>();
            setting.Pitch = pitch;

            SetSettings<TEnum>(setting);
        }

        public void SetPitch<TEnum>(TEnum sound, float pitch)
            where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            var setting = GetSettings(sound);
            setting.Pitch = pitch;

            SetSettings(sound, setting);
        }

        public float GetVolume()
        {
            return GetSettings().Volume;
        }

        public float GetVolume<TEnum>()
        {
            return GetSettings<TEnum>().Volume;
        }

        public float GetVolume<TEnum>(TEnum sound) where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            return GetSettings(sound).Volume;
        }

        public bool GetMute()
        {
            return GetSettings().Mute;
        }

        public bool GetMute<TEnum>()
        {
            return GetSettings<TEnum>().Mute;
        }

        public bool GetMute<TEnum>(TEnum sound) where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            return GetSettings(sound).Mute;
        }

        public float GetPitch()
        {
            return GetSettings().Pitch;
        }

        public float GetPitch<TEnum>()
        {
            return GetSettings<TEnum>().Pitch;
        }

        public float GetPitch<TEnum>(TEnum sound) where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            return GetSettings(sound).Pitch;
        }

        private void ApplySetting(SoundItem item)
        {
            var resultingSetting = GetResultingSetting(item.EnumType, item.EnumValue);
            item.SetSetting(resultingSetting);
        }

        private void SetSettings(SoundSetting setting)
        {
            GlobalSettings = setting;

            using (var list = GetPlayingAudioItemsCopy())
            {
                foreach (var item in list)
                {
                    ApplySetting(item);
                }
            }
        }

        private void SetSettings<TEnum>(SoundSetting setting)
        {
            using (var list = GetPlayingAudioItemsCopy<TEnum>())
            {
                var type = typeof(TEnum);
                GroupSettings[type] = setting;

                foreach (var item in list)
                {
                    ApplySetting(item);
                }
            }
        }

        private void SetSettings<TEnum>(TEnum sound, SoundSetting setting)
            where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            using (var list = GetPlayingAudioItemsCopy(sound))
            {
                var type = typeof(TEnum);
                var value = EnumInt32ToInt.Convert(sound);
                var key = new EnumComparerKey(type, value);

                SoundSettings[key] = setting;

                foreach (var item in list)
                {
                    ApplySetting(item);
                }
            }
        }

        private SoundSetting GetSettings()
        {
            return GlobalSettings;
        }

        private SoundSetting GetSettings<TEnum>()
        {
            var type = typeof(TEnum);

            if (GroupSettings.ContainsKey(type) == false)
                GroupSettings[type] = SoundSetting.Default;

            return GroupSettings[type];
        }

        private SoundSetting GetSettings<TEnum>(TEnum sound) where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            var type = typeof(TEnum);
            var value = EnumInt32ToInt.Convert(sound);
            var key = new EnumComparerKey(type, value);

            if (SoundSettings.ContainsKey(key) == false)
                SoundSettings[key] = SoundSetting.Default;

            return SoundSettings[key];
        }


        private SoundSetting GetResultingSetting(Type type, int value)
        {
            var key = new EnumComparerKey(type, value);

            var groupSetting = GroupSettings.ContainsKey(type) ? GroupSettings[type] : SoundSetting.Default;
            var soundSetting = SoundSettings.ContainsKey(key) ? SoundSettings[key] : SoundSetting.Default;

            var resultingSetting = new SoundSetting();

            resultingSetting.Mute = GlobalSettings.Mute || groupSetting.Mute || soundSetting.Mute;
            resultingSetting.Pitch = GlobalSettings.Pitch * groupSetting.Pitch * soundSetting.Pitch;
            resultingSetting.Volume = GlobalSettings.Volume * groupSetting.Volume * soundSetting.Volume;

            return resultingSetting;
        }

        public void Register<TEnum>(TEnum sound, AudioSource source) where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            using (var list = GetInternalAudioItemsCopy(sound))
            {
                var type = typeof(TEnum);
                var value = EnumInt32ToInt.Convert(sound);

                if (list.Count > 0)
                {
                    throw new Exception("External & internal sounds conflict! Such item already exists as internal!");
                }

                var newItem = new SoundItem(type, value, source, true);
                AudioItems.Add(newItem);

                newItem.Free = false;
            }
        }

        void IUpdatable.Update()
        {
            using (var list = GetPlayingAudioItemsCopy())
            {
                foreach (var item in list)
                {
                    item.Update();
                }
            };
        }

    }

}