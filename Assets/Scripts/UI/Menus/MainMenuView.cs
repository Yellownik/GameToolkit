using ButtonListeners;
using System;
using UnityEngine;

namespace UI.Menus
{
	public class MainMenuView : BaseMenuView
	{
		[SerializeField] private ButtonListener PlayButton;
		[SerializeField] private ButtonListener SettingsButton;
		[SerializeField] private ButtonListener ExitButton;

		public event Action PlayClicked
		{
			add => PlayButton.AddFunction(value);
			remove => PlayButton.RemoveFunctionsFromButton();
		}

		public event Action SettingsClicked
		{
			add => SettingsButton.AddFunction(value);
			remove => SettingsButton.RemoveFunctionsFromButton();
		}

		public event Action ExitClicked
		{
			add => ExitButton.AddFunction(value);
			remove => ExitButton.RemoveFunctionsFromButton();
		}
	}
}
