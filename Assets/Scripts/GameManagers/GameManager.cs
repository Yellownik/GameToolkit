using AudioSources;
using Core;
using Orbox.Utils;
using UI.Menus;
using UnityEngine;

namespace GameManagers
{
    public class GameManager
    {
        private readonly FadeManager _fadeManager;
        private readonly AudioManager _audioManager;
        private readonly MenuManager _menuManager;
        private readonly IResourceManager _resourceManager;

        private EGameLevels _levelType;
        private BaseLevel _gameLevel;

        public GameManager(FadeManager fadeManager, AudioManager audioManager, MenuManager menuManager, IResourceManager resourceManager)
        {
            _fadeManager = fadeManager;
            _audioManager = audioManager;
            _menuManager = menuManager;
            _resourceManager = resourceManager;
        }

        public void Run(EGameLevels gameLevel)
        {
            _levelType = gameLevel;
            
            _fadeManager.SetFade(true);
            _fadeManager.FadeIn(0.5f);
            _audioManager.PlayMusic(EMusic.Menu_Main, 0.5f);

            _menuManager.ShowMainMenu();
            _menuManager.WaitForPlay()
                .Done(StartTheGame);
            _menuManager.WaitForExit()
                .Done(ExitTheGame);
        }

        private void StartTheGame()
        {
            CreateLevel(_levelType);

            _gameLevel.StartLevel()
                .Done(ShowTitres);
        }

        private void CreateLevel(EGameLevels levelType)
        {
            var levelsRoot = new GameObject("GameLevels").transform;
            _gameLevel = _resourceManager.CreatePrefabInstance<EGameLevels, BaseLevel>(levelType, levelsRoot);
            _gameLevel.gameObject.SetActive(false);
        }
        
        private void ShowTitres()
        {
            _audioManager.PlayMusic(EMusic.Titres, fadeTime: 1);
            _menuManager.ShowTitresMenu();
            _audioManager.StopMusic(delay: 5, fadeTime: 2);

            _fadeManager.FadeIn(duration: 2)
                .Then(() => _fadeManager.FadeOut(delay: 3, duration: 2))
                .Done(ExitTheGame);
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
