using System;
using System.Collections.Generic;
using UnityEngine;

namespace ButtonListeners
{
    public enum EButtonAction
    {
        Click,
        MouseDown,
        MouseUp,
        MouseEnter,
        MouseExit,
    }

    public static class ButtonEventListener
    {
        public static bool IsActive { get; private set; } = true;

        private static Dictionary<ButtonListener, Action> Buttons = new Dictionary<ButtonListener, Action>();
        private static List<ButtonListener> ExceptionButtonsForDisable = new List<ButtonListener>();

        public static void SetActiveState(bool isActive)
        {
            IsActive = isActive;
        }

        public static void AddExceptionButton(this ButtonListener _button)
        {
            if (!ExceptionButtonsForDisable.Contains(_button))
                ExceptionButtonsForDisable.Add(_button);
        }

        #region Click
        public static void AddFunction(this ButtonListener button, Action callBack, EButtonAction buttonAction = EButtonAction.Click)
        {
            if (Buttons.ContainsKey(button))
                RemoveFuncFromButton(button);

            Buttons.Add(button, callBack);

            switch (buttonAction)
            {
                case EButtonAction.Click:
                    button.OnClick += () => RiseEventOnButton(button);
                    break;

                case EButtonAction.MouseDown:
                    button.OnDown += () => RiseEventOnButton(button);
                    break;

                case EButtonAction.MouseUp:
                    button.OnUp += () => RiseEventOnButton(button);
                    break;

                case EButtonAction.MouseEnter:
                    button.OnEnter += () => RiseEventOnButton(button);
                    break;

                case EButtonAction.MouseExit:
                    button.OnExit += () => RiseEventOnButton(button);
                    break;
            }
        }

        private static void RiseEventOnButton(ButtonListener button)
        {
            if (Buttons.ContainsKey(button) && Buttons[button] != null)
            {
                if (IsActive)
                {
                    bool exceptionButtons = ExceptionButtonsForDisable.Contains(button);
                    if (!exceptionButtons)
                    {
                        Debug.Log("All Buttons Are Disabled");
                        return;
                    }
                    
                    Debug.Log(button.name + "is exception button for disable");
                }

                Buttons[button]();
            }
        }

        public static void RemoveFunctionsFromButton(this ButtonListener button)
        {
            RemoveFuncFromButton(button);
        }

        private static void RemoveFuncFromButton(ButtonListener button)
        {
            if (Buttons.ContainsKey(button))
            {
                button.ClearEvents();
                Buttons.Remove(button);
            }
        }
        #endregion

        public static void Reset()
        {
            SetActiveState(true);

            foreach (var button in Buttons)
                button.Key.ClearEvents();

            Buttons.Clear();
            ExceptionButtonsForDisable.Clear();
        }
    }
}