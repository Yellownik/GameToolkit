using AudioSources;
using Orbox.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
	public class Root : MonoBehaviour
	{
		public static IResourceManager ResourceManager { get; private set; }
		public static ITimerService TimerService { get; private set; }
		public static InputManager InputManager { get; private set; }

		public static AudioManager AudioManager { get; private set; }


		private static Transform RootTransform;

		private void Awake()
		{
			RootTransform = transform;

			Init();
		}

		private void Init()
		{
			ResourceManager = new ResourceManager();
			TimerService = MonoExtensions.MakeComponent<TimerService>(RootTransform);
			InputManager = MonoExtensions.MakeComponent<InputManager>(RootTransform);

			AudioManager = CreateAudioManager(ResourceManager, TimerService);
		}

		private static AudioManager CreateAudioManager(IResourceManager resourceManager, ITimerService timerService)
		{
			var audioSourcesParent = new GameObject("AudioSources").transform;
			audioSourcesParent.SetParent(RootTransform);
			var soundManager = new SoundManager(resourceManager, timerService, audioSourcesParent);

			var audioManager = ResourceManager.CreatePrefabInstance<EComponents, AudioManager>(EComponents.AudioManager, RootTransform);
			audioManager.Init(soundManager, timerService);
			return audioManager;
		}
	}
}
