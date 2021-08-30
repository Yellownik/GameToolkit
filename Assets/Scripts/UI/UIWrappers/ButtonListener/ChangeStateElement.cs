using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ButtonListeners
{
    public enum ChangeStateEvent
    {
        ButtonPressed,
        MouseOver,

        ElementSelected,
        ElementLocked,
    }

    public enum ChangeStateType
    {
        LocalScale,
        LocalPosition,
        ColorTint,
        Color,
        Sprite,
        OnOff,
    }

    [ExecuteAlways]
    public class ChangeStateElement : MonoBehaviour
    {
        [System.Serializable]
        public class Data
        {
            public ChangeStateEvent EventToReceive = ChangeStateEvent.ButtonPressed;
            public ChangeStateType ChangeStateType = ChangeStateType.LocalScale;

            public Vector3 Scale = Vector3.one * 0.95f;
            public Vector3 Position = Vector3.up * 1;

            public float ColorTint = 0.95f;
            public Color Color = Color.white;
            public Sprite Sprite;

            public bool IsActive;
            public float Duration;
        }
       
        [SerializeField] private bool IsAllowInitOnAwake = true;

        public Image ImageTarget { get; private set; }
        public TextMeshProUGUI TextTarget { get; private set; }

        public List<Data> dataList = new List<Data>() { new Data() };
        private Data Normal;

        private bool IsInited = false;

        private void Awake()
        {
            ImageTarget = GetComponent<Image>();
            TextTarget = GetComponent<TextMeshProUGUI>();

            if (IsAllowInitOnAwake)
                InitOnChanged();
        }

        private void InitOnChanged()
        {
            Normal = new Data
            {
                Scale = transform.localScale,
                Position = transform.localPosition,

                ColorTint = 1,
                Color = ImageTarget != null ? ImageTarget.color : Color.white,
                Sprite = ImageTarget != null ? ImageTarget.sprite : null,

                IsActive = gameObject.activeSelf,
            };

            IsInited = true;
        }

        public void RevertToDefault()
        {
            ChangeStateByEvent(Normal, false);
        }

        public virtual void ReceiveEvent(ChangeStateEvent stateEvent, bool state)
        {
            if (!IsInited)
                InitOnChanged();

            for (int i = 0; i < dataList.Count; i++)
            {
                if (dataList[i].EventToReceive == stateEvent)
                    ChangeStateByEvent(dataList[i], state);
            }
        }

        protected void ChangeStateByEvent(Data data, bool state)
        {
            switch (data.ChangeStateType)
            {
                case ChangeStateType.LocalScale:
                    var resScale = Normal.Scale;
                    if (state)
                        resScale.Scale(data.Scale);

                    if (data.Duration > 0)
                        transform.DOScale(resScale, data.Duration);
                    else
                        transform.localScale = resScale;
                    break;

                case ChangeStateType.LocalPosition:
                    var resPos = state ? Normal.Position + data.Position : Normal.Position;

                    if (data.Duration > 0)
                        transform.DOLocalMove(resPos, data.Duration);
                    else
                        transform.localPosition = resPos;
                    break;

                case ChangeStateType.ColorTint:
                    var resTintColor = state ? Normal.Color * data.ColorTint : Normal.Color;
                    ChangeColor(resTintColor, data.Duration);
                    break;

                case ChangeStateType.Color:
                    var resColor = state ? data.Color : Normal.Color;
                    ChangeColor(resColor, data.Duration);
                    break;

                case ChangeStateType.Sprite:
                    if (ImageTarget != null)
                        ImageTarget.sprite = state ? data.Sprite : Normal.Sprite;
                    break;

                case ChangeStateType.OnOff:
                    var active = state ? data.IsActive : Normal.IsActive;
                    gameObject.SetActive(active);
                    break;
            }
        }

        private void ChangeColor(Color color, float duration)
        {
            if (ImageTarget != null)
            {
                if (duration > 0)
                    ImageTarget.DOColor(color, duration);
                else
                    ImageTarget.color = color;
            }

            if (TextTarget != null)
            {
                if (duration > 0)
                    TextTarget.DOColor(color, duration);
                else
                    TextTarget.color = color;
            }
        }

#if UNITY_EDITOR
        public void CreateData_Editor()
        {
            dataList.Add(new Data());
        }

        public void RemoveData_Editor()
        {
            if (dataList.Count > 0)
                dataList.Remove(dataList[dataList.Count - 1]);
        }
#endif
	}
}