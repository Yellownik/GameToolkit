using AudioSources;
using ButtonListeners;
using Core;
using FlyTexts;
using UI.Menus;
using UnityEngine;

namespace GameManagers
{
    public class DemoLevel : MonoBehaviour
    {
        [SerializeField] private ButtonListener Button;
        [SerializeField] private int ClicksToEnd = 10;
        [SerializeField] private float RandomRange = 100;
        [SerializeField] private bool IsUniqueFlyText = true;

        private FadeManager FadeManager => Root.FadeManager;
        private AudioManager AudioManager => Root.AudioManager;
        private MenuManager MenuManager => Root.MenuManager;
        private ITimerService TimerService => Root.TimerService;
        private GameManager GameManager => Root.GameManager;
        private FlyTextManager FlyTextManager => Root.FlyTextManager;

        void Start()
        {
            Button.AddFunction(OnClick);
        }

        public void StartLevel()
        {
            gameObject.SetActive(true);
            FadeManager.FadeIn();
        }

        private void OnClick()
        {
            if(ClicksToEnd <= 0)
            {
                AudioManager.StopMusic(fadeTime: 1);
                FadeManager.ResetFadeCenter();
                FadeManager.FadeOut(duration: 1)
                    .Done(EndTheGame);
                return;
            }

            FlyTextManager.Spawn(Button.transform, ClicksToEnd.ToString(), Random.insideUnitCircle * RandomRange, isUnique: IsUniqueFlyText);
            ClicksToEnd--;
        }

        private void EndTheGame()
        {
            AudioManager.PlayMusic(EMusic.Titres, fadeTime: 1);
            MenuManager.ShowTitresMenu();
            AudioManager.StopMusic(delay: 5, fadeTime: 2);

            FadeManager.FadeIn(duration: 2)
                .Then(() => FadeManager.FadeOut(delay: 3, duration: 2))
                .Then(() => TimerService.Wait(0.5f))
                .Done(() => GameManager.ExitTheGame());
        }
    }
}
