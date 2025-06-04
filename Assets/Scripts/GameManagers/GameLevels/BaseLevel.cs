using AudioSources;
using Core;
using Orbox.Async;
using UnityEngine;

namespace GameManagers
{
    public abstract class BaseLevel : MonoBehaviour
    {
        private static FadeManager FadeManager => Root.FadeManager;
        protected static AudioManager AudioManager => Root.AudioManager;

        private Promise _endLevelPromise = new ();
        
        public virtual IPromise StartLevel()
        {
            gameObject.SetActive(true);
            FadeManager.FadeIn();

            return _endLevelPromise;
        }
        
        protected virtual void EndLevel()
        {
            AudioManager.StopMusic(fadeTime: 1);

            FadeManager.ResetFadeCenter();
            FadeManager.FadeOut(duration: 1)
                .Done(() => _endLevelPromise.Resolve());
        }
    }
}