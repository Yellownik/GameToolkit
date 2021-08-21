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
            public float DurationAnim;
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
                case ChangeStateType.Color:
                    if (ImageTarget != null)
                        ImageTarget.color = state ? data.Color : Normal.Color;

                    if (TextTarget != null)
                        TextTarget.color = state ? data.Color : Normal.Color;
                    break;

                case ChangeStateType.ColorTint:
                    if (ImageTarget != null)
                        ImageTarget.color = state ? Normal.Color * data.ColorTint : Normal.Color;

                    if (TextTarget != null)
                        TextTarget.color = state ? Normal.Color * data.ColorTint : Normal.Color;
                    break;

                case ChangeStateType.LocalPosition:
                    transform.localPosition = state ? Normal.Position + data.Position : Normal.Position;
                    break;

                case ChangeStateType.LocalScale:
                    Vector3 resScale = Normal.Scale;
                    if (state) 
                        resScale.Scale(data.Scale);

                    transform.localScale = resScale;
                    break;

                case ChangeStateType.OnOff:
                    var active = state ? data.IsActive : Normal.IsActive;
                    gameObject.SetActive(active);
                    break;

                case ChangeStateType.Sprite:
                    if (ImageTarget != null)
                        ImageTarget.sprite = state ? data.Sprite : Normal.Sprite;
                    break;
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