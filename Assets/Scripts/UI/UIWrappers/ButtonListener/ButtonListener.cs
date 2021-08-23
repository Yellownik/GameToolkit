using AudioSources;
using Core;
using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ButtonListeners
{
    [ExecuteAlways]
    [SelectionBase]
    [DisallowMultipleComponent]
    public class ButtonListener : UIBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, 
        IPointerExitHandler, IPointerEnterHandler
    {
        [SerializeField] private bool Interactable = true;
        [SerializeField] private bool PlaySound = true;
        [SerializeField] private ESounds ClickSound = ESounds.Click;

        [SerializeField] private List<ChangeStateElement> ChangeStateElements;

        [Space]
        [SerializeField] private ChangeStateEvent ChangeStateEvent;
        [SerializeField] private bool IsStateActive = true;

        public event Action OnClick = () => { };

        public event Action OnDown = () => { };
        public event Action OnUp = () => { };
        
        public event Action OnEnter = () => { };
        public event Action OnExit = () => { };

        public bool ChosenState { get; private set; }
        public bool LockedState { get; private set; }

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
                Root.AudioManager.PlaySound(ClickSound);

            IsPointerDown = true;
            BroadcastEvent(ChangeStateEvent.ButtonPressed, IsPointerDown);
            OnDown();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            if (!IsActive() || !Interactable)
                return;

            IsPointerDown = false;
            BroadcastEvent(ChangeStateEvent.ButtonPressed, IsPointerDown);
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
            BroadcastEvent(ChangeStateEvent.MouseOver, IsPointerInside);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            if (!IsActive() || !Interactable)
                return;

            OnExit();
            IsPointerInside = false;
            BroadcastEvent(ChangeStateEvent.MouseOver, IsPointerInside);
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
        private void BroadcastEvent_Editor()
        {
            BroadcastEvent(ChangeStateEvent, IsStateActive);
            IsStateActive = !IsStateActive;
        }

        [Button]
        private void InitChangeElements()
        {
            gameObject.GetComponentsInChildren<ChangeStateElement>(true, ChangeStateElements);
        }
    }
}