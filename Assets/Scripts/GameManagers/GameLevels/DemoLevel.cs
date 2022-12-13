using AudioSources;
using ButtonListeners;
using Core;
using FlyTexts;
using Orbox.Async;
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
        private FlyTextManager FlyTextManager => Root.FlyTextManager;

        private Promise EndLevelPromise = new Promise();
        
        private void Start()
        {
            Button.AddFunction(OnClick);
        }

        public IPromise StartLevel()
        {
            gameObject.SetActive(true);
            FadeManager.FadeIn();

            return EndLevelPromise;
        }

        private void OnClick()
        {
            if (ClicksToEnd <= 0)
            {
                EndLevel();
            }
            else
            {
                SpawnFlyText();
                ClicksToEnd--;
            }
        }

        private void EndLevel()
        {
            AudioManager.StopMusic(fadeTime: 1);

            FadeManager.ResetFadeCenter();
            FadeManager.FadeOut(duration: 1)
                .Done(() => EndLevelPromise.Resolve());
        }

        private void SpawnFlyText()
        {
            var offset = Random.onUnitSphere * RandomRange;
            offset.z = Button.transform.position.z;

            FlyTextManager.Spawn(Button.GetComponent<RectTransform>(), ClicksToEnd.ToString(), offset, isUnique: IsUniqueFlyText);
        }
    }
}
