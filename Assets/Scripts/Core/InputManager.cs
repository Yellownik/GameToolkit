using ButtonListeners;
using System;
using UnityEngine;

namespace Core
{
	public class InputManager : MonoBehaviour
	{
		public bool IsActive { get; private set; } = true;

		public event Action GamePausing = () => { };

		public void SetActiveState(bool isActive)
		{
			IsActive = isActive;
			ButtonEventListener.SetActiveState(isActive);
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				GamePausing();
			}
		}
	}
}
