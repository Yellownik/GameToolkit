﻿using DG.Tweening;
using NaughtyAttributes;
using Orbox.Async;
using System.Collections;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    public class FadeManager : MonoBehaviour
    {
        [SerializeField] private float FadeTime = 0.5f;

        private CameraManager CameraManager;
        private InputManager InputManager;
        private Image FadeImage;
        
        private static readonly int CenterPoint = Shader.PropertyToID("_CenterPoint");
        private static readonly int DissolveValue = Shader.PropertyToID("_DissolveValue");

        public bool IsFading { get; private set; } = false;

        public void Init(CameraManager cameraManager, InputManager inputManager, IUIRoot uiRoot)
        {
            CameraManager = cameraManager;
            InputManager = inputManager;

            FadeImage = uiRoot.FadeImage;
            FadeImage.material = Instantiate(FadeImage.material);
        }

        #region Fading
        public void ResetFadeCenter()
        {
            FadeImage.material.SetVector(CenterPoint, new Vector4(0.5f, 0.5f));
        }

        public void SetFadeCenterToPosition(Vector3 worldPos)
        {
            var viewPoint = CameraManager.Camera.WorldToViewportPoint(worldPos);
            FadeImage.material.SetVector(CenterPoint, new Vector4(viewPoint.x, viewPoint.y));
        }

        public void SetFade(bool isFade)
        {
            if (isFade)
                SetDissolveValue(1);
            else
                SetDissolveValue(0);

            FadeImage.gameObject.SetActive(isFade);
        }

        public IPromise FadeOut(float delay = 0, float duration = -1)
        {
            IsFading = true;
            FadeImage.gameObject.SetActive(true);
            InputManager.SetActiveState(false);

            var promise = new Promise();
            float fadeTime = duration > 0 ? duration : FadeTime;
            
            DOTween.To(SetDissolveValue, 0, 1, fadeTime)
                .SetEase(Ease.Linear)
                .SetDelay(delay)
                .OnComplete(() => promise.Resolve());

            return promise;
        }

        public IPromise FadeIn(float delay = 0, float duration = -1)
        {
            IsFading = true;
            float fadeTime = duration > 0 ? duration : FadeTime;

            var promise = new Promise();
            DOTween.To(SetDissolveValue, 1, 0, fadeTime)
                .SetEase(Ease.Linear)
                .SetDelay(delay)
                .OnComplete(() =>
                {
                    IsFading = false;
                    FadeImage.gameObject.SetActive(false);
                    InputManager.SetActiveState(true);

                    promise.Resolve();
                });

            return promise;
        }

        private void SetDissolveValue(float value)
        {
            FadeImage.material.SetFloat(DissolveValue, value);
        }
        #endregion

#if UNITI_EDITOR
        [Button]
        public void FadeInOut_Editor()
        {
            FadeImage.gameObject.SetActive(true);

            var seq = DOTween.Sequence();
            seq.Append(DOTween.To(SetDissolveValue, 0, 1, FadeTime).SetEase(Ease.Linear));
            seq.Append(DOTween.To(SetDissolveValue, 1, 0, FadeTime).SetEase(Ease.Linear));
            
            seq.AppendCallback(() => FadeImage.gameObject.SetActive(false));
        }
#endif
    }
}
