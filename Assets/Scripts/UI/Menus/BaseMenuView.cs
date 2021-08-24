using DG.Tweening;
using Orbox.Async;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace UI.Menus
{
    [RequireComponent(typeof(RectTransform))]
    public abstract class BaseMenuView : MonoBehaviour
    {
        [SerializeField] protected CanvasGroup CanvasGroup;
        [SerializeField] protected List<Transform> TransformsForScale;

        public bool IsShown { get; protected set; }
        public bool IsAnimating { get; protected set; }

        #region Show/Hide
        public virtual void Enable()
        {
            IsShown = true;
            gameObject.SetActive(true);
        }

        public virtual void Disable()
        {
            IsShown = false;
            gameObject.SetActive(false);
        }

        public virtual IPromise Show()
        {
            Enable();
            return ShowAnimated(true);
        }

        public virtual IPromise Hide()
        {
            return ShowAnimated(false)
                        .Done(Disable);
        }

        protected virtual IPromise ShowAnimated(bool isShow)
        {
            IsAnimating = true;
            var promise = new Promise();

            foreach (var trans in TransformsForScale)
                trans.DoScale(isShow);

            CanvasGroup.DoFade(isShow)
                .OnComplete(() =>
                {
                    IsAnimating = false;
                    promise.Resolve();
                });

            return promise;
        }
        #endregion

        public virtual void SetParent(RectTransform parent)
        {
            transform.SetParent(parent, false);
        }

        public virtual void SetParent(Transform parent)
        {
            transform.SetParent(parent, false);
        }
    }
}
