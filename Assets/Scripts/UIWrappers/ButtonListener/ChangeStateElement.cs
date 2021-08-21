using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ButtonListeners
{
    public enum ChangeStateType
    {
        Color,
        ColorTint,
        LocalPosition,
        LocalScale,
        Image,
        OnOff,
    }

    [ExecuteAlways]
    public class ChangeStateElement : MonoBehaviour
    {
        public ChangeStateEvent EventToReceive = ChangeStateEvent.ButtonPressed;
        public ChangeStateType ChangeStateType = ChangeStateType.LocalScale;
       
        public Image ImageTarget;
        public TextMeshProUGUI TextTarget;

        public Color NormalColor = Color.white;
        public Color StateColor = Color.white;
        
        private Vector3 NormalScale;
        public Vector3 StateScaleMult = new Vector3(0.95f, 0.95f, 0.95f);

        private Vector3 NormalPosition;
        public Vector3 StatePositionShift;

        private Sprite NormalSprite;
        public Sprite StateSprite;

        public float StateColorTint = 0.95f;
        public bool StateActive;

        public bool IsAllowInitOnAwake = true;
        private bool IsInited = false;

        private void Awake()
        {
            if (!IsAllowInitOnAwake)
                return;

            ImageTarget = GetComponent<Image>();
            TextTarget = GetComponent<TextMeshProUGUI>();
        }

        private void InitOnChanged()
        {
            if (ImageTarget != null)
                NormalSprite = ImageTarget.sprite;

            NormalScale = transform.localScale;
            NormalPosition = transform.localPosition;
            IsInited = true;
        }

        public void ReceiveEvent(ChangeStateEvent stateEvent, bool state)
        {
            if (EventToReceive == stateEvent)
            {
                if (!IsInited)
                    InitOnChanged();

                ChangeStateByEvent(state);
            }
        }

        public void ChangeStateByEvent(bool state)
        {
            switch (ChangeStateType)
            {
                case ChangeStateType.Color:
                    if (ImageTarget != null)
                        ImageTarget.color = state ? StateColor : NormalColor;

                    if (TextTarget != null)
                        TextTarget.color = state ? StateColor : NormalColor;
                    break;

                case ChangeStateType.ColorTint:
                    if (ImageTarget != null)
                        ImageTarget.color = state ? NormalColor * StateColorTint : NormalColor;
                    break;

                case ChangeStateType.LocalPosition:
                    transform.localPosition = state ? NormalPosition + StatePositionShift : NormalPosition;
                    break;

                case ChangeStateType.LocalScale:
                    Vector3 resScale = NormalScale;
                    if (state) 
                        resScale.Scale(StateScaleMult);

                    transform.localScale = resScale;
                    break;

                case ChangeStateType.OnOff:
                    gameObject.SetActive(state ? StateActive : !StateActive);
                    break;

                case ChangeStateType.Image:
                    if (ImageTarget != null)
                        ImageTarget.sprite = state ? StateSprite : NormalSprite;
                    break;
            }
        }

        public void RevertToDefault()
        {
            ChangeStateByEvent(false);
        }
    }
}