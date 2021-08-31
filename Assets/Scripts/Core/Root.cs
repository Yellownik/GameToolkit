using AudioSources;
using FlyTexts;
using GameManagers;
using Orbox.Utils;
using Saving;
using System.Collections;
using System.Collections.Generic;
using UI;
using UI.Menus;
using UnityEngine;

namespace Core
{
	public class Root : MonoBehaviour
	{
		public static IResourceManager ResourceManager { get; private set; }
		public static IUIRoot UIRoot { get; private set; }
		public static ViewFactory ViewFactory { get; private set; }

		public static ITimerService TimerService { get; private set; }
		public static InputManager InputManager { get; private set; }


		public static AudioManager AudioManager { get; private set; }
		public static CameraManager CameraManager { get; private set; }
		public static FadeManager FadeManager { get; private set; }
		public static MenuManager MenuManager { get; private set; }
		public static FlyTextManager FlyTextManager { get; private set; }

		public static GameManager GameManager { get; private set; }
		public static ISaveManager SaveManager { get; private set; }

		private static Transform RootTransform;

		private void Awake()
		{
			RootTransform = transform;

			Init();
		}

		private void Init()
		{
			ResourceManager = new ResourceManager();
			UIRoot = ResourceManager.CreatePrefabInstance<EComponents, UIRoot>(EComponents.UIRoot, RootTransform);
			ViewFactory = new ViewFactory(ResourceManager, UIRoot);
			FlyTextManager = new FlyTextManager(ResourceManager);

			TimerService = MonoExtensions.MakeComponent<TimerService>(RootTransform);
			InputManager = new InputManager(TimerService);

			AudioManager = CreateAudioManager(ResourceManager, TimerService);
			CameraManager = MonoExtensions.MakeComponent<CameraManager>(RootTransform);

			FadeManager = ResourceManager.CreatePrefabInstance<EComponents, FadeManager>(EComponents.FadeManager, RootTransform);
			FadeManager.Init(CameraManager, InputManager, UIRoot);

			SaveManager = new SaveManager();
			SaveManager.RestoreData();

			MenuManager = new MenuManager(ViewFactory, InputManager, FadeManager, SaveManager, AudioManager);
			GameManager = CreateGameManager();
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

		private static GameManager CreateGameManager()
		{
			var gameManager = new GameManager();
			return gameManager;
		}
	}
}
