using ButtonListeners;
using System;
using UnityEngine;

namespace Core
{
	public class InputManager
	{
		public bool IsActive { get; private set; } = true;

		public event Action GamePausing = () => { };

		public InputManager(ITimerService timerService)
		{
			timerService.SubscribeForUpdate(InputUpdate);
		}

		public void SetActiveState(bool isActive)
		{
			IsActive = isActive;
			ButtonEventListener.SetActiveState(isActive);
		}

		private void InputUpdate()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				GamePausing();
			}
		}
	}
}
