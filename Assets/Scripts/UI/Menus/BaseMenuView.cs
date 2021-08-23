using Orbox.Async;
using UnityEngine;

namespace UI.Menus
{
    [RequireComponent(typeof(RectTransform))]
    public abstract class BaseMenuView : MonoBehaviour
    {
        private RectTransform _RectTransform;
        protected RectTransform RectTransform
        {
            get
            {
                if (_RectTransform == null)
                    _RectTransform = GetComponent<RectTransform>();

                return _RectTransform;
            }
        }

        public virtual void Disable()
        {
            gameObject.SetActive(false);
        }

        public virtual void Enable()
        {
            gameObject.SetActive(true);
        }

        public virtual IPromise Hide()
        {
            Disable();
            return new Promise().Resolve();
        }

        public virtual IPromise Show()
        {
            Enable();
            return new Promise().Resolve();
        }

        public virtual void SetParent(RectTransform parent)
        {
            transform.SetParent(parent, false);
        }

        public virtual void SetParent(Transform parent)
        {
            transform.SetParent(parent, false);
        }

        public void SetAnchoredPosition(Vector3 anchoredPosition)
        {
            RectTransform.anchoredPosition = anchoredPosition;
        }
    }
}
