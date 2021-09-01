using AudioSources;
using Core;
using Orbox.Utils;
using UI.Menus;
using UnityEngine;

namespace GameManagers
{
    public class GameManager
    {
        private readonly FadeManager FadeManager;
        private readonly AudioManager AudioManager;
        private readonly MenuManager MenuManager;

        private DemoLevel DemoLevel;

        public GameManager(FadeManager fadeManager, AudioManager audioManager, MenuManager menuManager, IResourceManager resourceManager)
        {
            FadeManager = fadeManager;
            AudioManager = audioManager;
            MenuManager = menuManager;

            InitLevels(resourceManager);
        }

        private void InitLevels(IResourceManager resourceManager)
        {
            var levelsRoot = new GameObject("GameLevels").transform;
            DemoLevel = resourceManager.CreatePrefabInstance<EGameLevels, DemoLevel>(EGameLevels.DemoLevel, levelsRoot);
            DemoLevel.gameObject.SetActive(false);
        }

        public void Run()
        {
            FadeManager.SetFade(true);
            FadeManager.FadeIn(0.5f);
            AudioManager.PlayMusic(EMusic.Menu_Main, 0.5f);

            MenuManager.ShowMainMenu();
            MenuManager.WaitForPlay()
                .Done(StartTheGame);
        }

        private void StartTheGame()
        {
            Debug.Log("Game started");
            DemoLevel.StartLevel();
        }

        public void ExitTheGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            UnityEngine.Application.Quit();
#endif
        }
    }
}
