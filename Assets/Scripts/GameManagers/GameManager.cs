using AudioSources;
using Core;
using UI.Menus;
using UnityEngine;

namespace GameManagers
{
    public class GameManager
    {
        private readonly FadeManager FadeManager;
        private readonly AudioManager AudioManager;
        private readonly MenuManager MenuManager;

        public GameManager(FadeManager fadeManager, AudioManager audioManager, MenuManager menuManager)
        {
            FadeManager = fadeManager;
            AudioManager = audioManager;
            MenuManager = menuManager;
        }

        public void Run()
        {
            FadeManager.SetFade(true);
            FadeManager.FadeIn(0.5f);
            AudioManager.PlayMusic(EMusic.Main_Menu, 0.5f);

            MenuManager.ShowMainMenu();
            MenuManager.WaitForPlay()
                .Done(StartTheGame);
        }

        private void StartTheGame()
        {
            AudioManager.StopMusic();

            Debug.Log("Game started");
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
