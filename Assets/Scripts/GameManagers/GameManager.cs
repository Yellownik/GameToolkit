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
            MenuManager.WaitForExit()
                .Done(ExitTheGame);
        }

        private void StartTheGame()
        {
            DemoLevel.StartLevel()
                .Done(ShowTitres);
        }

        private void ShowTitres()
        {
            AudioManager.PlayMusic(EMusic.Titres, fadeTime: 1);
            MenuManager.ShowTitresMenu();
            AudioManager.StopMusic(delay: 5, fadeTime: 2);

            FadeManager.FadeIn(duration: 2)
                .Then(() => FadeManager.FadeOut(delay: 3, duration: 2))
                .Done(() => ExitTheGame());
        }
        
        private void ExitTheGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            UnityEngine.Application.Quit();
#endif
        }
    }
}
