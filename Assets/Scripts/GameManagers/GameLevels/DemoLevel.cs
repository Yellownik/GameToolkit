using ButtonListeners;
using Core;
using UnityEngine;

namespace GameManagers
{
    public class DemoLevel : MonoBehaviour
    {
        [SerializeField] private ButtonListener Button;
        [SerializeField] private int ClicksToEnd = 10;
        [SerializeField] private float RandomRange = 100;
        [SerializeField] private bool IsUniqueFlyText = true;

        void Start()
        {
            Button.AddFunction(OnClick);
        }

        public void StartLevel()
        {
            gameObject.SetActive(true);
            Root.FadeManager.FadeIn();
        }

        private void OnClick()
        {
            if(ClicksToEnd <= 0)
            {
                Root.AudioManager.StopMusic(fadeTime: 1);
                Root.FadeManager.ResetFadeCenter();
                Root.FadeManager.FadeOut(duration: 1)
                    .Done(EndTheGame);
                return;
            }

            Root.FlyTextManager.Spawn(Button.transform, ClicksToEnd.ToString(), Random.insideUnitCircle * RandomRange, isUnique: IsUniqueFlyText);
            ClicksToEnd--;
        }

        private void EndTheGame()
        {
            Root.AudioManager.PlayMusic(AudioSources.EMusic.Titres, fadeTime: 1);
            Root.MenuManager.ShowTitresMenu();
            Root.AudioManager.StopMusic(delay: 5, fadeTime: 2);

            Root.FadeManager.FadeIn(duration: 2)
                .Then(() => Root.FadeManager.FadeOut(delay: 3, duration: 2))
                .Then(() => Root.TimerService.Wait(0.5f))
                .Done(() => Root.GameManager.ExitTheGame());
        }
    }
}
