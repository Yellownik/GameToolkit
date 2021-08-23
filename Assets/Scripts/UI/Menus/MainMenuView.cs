using Orbox.Async;
using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menus
{
	public class MainMenuView : BaseMenuView
	{
		[SerializeField] private Button PlayButton;
		[SerializeField] private Button SettingsButton;
		[SerializeField] private Button ExitButton;

		public Button.ButtonClickedEvent PlayClickEvent => PlayButton.onClick;
		public Button.ButtonClickedEvent SettingsClickEvent => SettingsButton.onClick;
		public Button.ButtonClickedEvent ExitClickEvent => ExitButton.onClick;
	}
}
