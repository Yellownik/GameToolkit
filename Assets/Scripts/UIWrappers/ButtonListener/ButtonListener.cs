using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ButtonListeners
{
    public enum ChangeStateEvent
    {
        ElementSelected,
        ElementLocked,
        ButtonPressed,
        MouseOver
    }

    [ExecuteAlways]
    [SelectionBase]
    [DisallowMultipleComponent]
    public class ButtonListener : UIBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, 
        IPointerExitHandler, IPointerEnterHandler
    {
        [SerializeField] private bool Interactable = true;
        [SerializeField] private bool PlaySound = true;

        [SerializeField] private bool ChosenState;
        [SerializeField] private bool LockedState;

        [SerializeField] private List<ChangeStateElement> ChangeStateElements;

        public event Action OnClick = () => { };

        public event Action OnDown = () => { };
        public event Action OnUp = () => { };
        
        public event Action OnEnter = () => { };
        public event Action OnExit = () => { };

        private bool IsPointerDown;
        private bool IsPointerInside;

        public void SetChosen(bool chosen)
        {
            ChosenState = chosen;
            BroadcastEvent(ChangeStateEvent.ElementSelected, chosen);
        }

        public void SetLocked(bool locked, bool disable = false)
        {
            LockedState = locked;
            if (disable)
                Interactable = !LockedState;

            BroadcastEvent(ChangeStateEvent.ElementLocked, LockedState);
        }

        public bool IsHighlighted()
        {
            if (!IsActive() || !Interactable)
                return false;

            return IsPointerInside && !IsPointerDown;
        }

        public bool IsPressed()
        {
            if (!IsActive() || !Interactable)
                return false;

            return IsPointerDown;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            if (!IsActive() || !Interactable)
                return;

            OnClick();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            if (!IsActive() || !Interactable)
                return;

            // Selection tracking
            if (Interactable && EventSystem.current != null)
                EventSystem.current.SetSelectedGameObject(gameObject, eventData);

            if (PlaySound)
                Debug.Log("No click sound");
                //SoundManager.Instance.PlaySound(_soundName);

            IsPointerDown = true;
            BroadcastEvent(ChangeStateEvent.ButtonPressed, true);
            OnDown();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            if (!IsActive() || !Interactable)
                return;

            IsPointerDown = false;
            BroadcastEvent(ChangeStateEvent.ButtonPressed, false);
            OnUp();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            if (!IsActive() || !Interactable)
                return;

            OnEnter();
            IsPointerInside = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            if (!IsActive() || !Interactable)
                return;

            OnExit();
            BroadcastEvent(ChangeStateEvent.ButtonPressed, false);
            IsPointerInside = false;
        }

        private void BroadcastEvent(ChangeStateEvent eventType, bool state)
        {
            foreach (var elementItem in ChangeStateElements)
                elementItem.ReceiveEvent(eventType, state);
        }

        public void ClearEvents()
        {
            OnClick = () => { };

            OnDown = () => { };
            OnUp = () => { };

            OnEnter = () => { };
            OnExit = () => { };
        }

        [Button]
        public void ChangeLockState()
        {
            SetLocked(!LockedState);
        }

        [Button]
        private void InitChangeElements()
        {
            gameObject.GetComponentsInChildren<ChangeStateElement>(true, ChangeStateElements);
        }
    }
}