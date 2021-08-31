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
                Root.FadeManager.FadeOut(duration: 2)
                    .Done(EndTheGame);
                return;
            }

            Root.FlyTextManager.Spawn(Button.transform, ClicksToEnd.ToString(), Random.insideUnitCircle * RandomRange, isUnique: IsUniqueFlyText);
            ClicksToEnd--;
        }

        private void EndTheGame()
        {
            Root.FadeManager.FadeIn(duration: 2);
            Root.MenuManager.ShowTitresMenu();

            Root.AudioManager.StopMusic(delay: 3, fadeTime: 2);
            Root.FadeManager.FadeOut(delay: 3, duration: 2)
                .Then(() => Root.TimerService.Wait(1))
                .Done(() => Root.GameManager.ExitTheGame());
        }
    }
}
