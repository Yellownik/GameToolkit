using UnityEngine;
using UnityEngine.UI;

namespace UI.Menus
{
	public class TitresMenuView : BaseMenuView
	{
		[SerializeField] private Button PlayButton;
		[SerializeField] private Button SettingsButton;
		[SerializeField] private Button ExitButton;

		public Button.ButtonClickedEvent PlayClickEvent => PlayButton.onClick;
		public Button.ButtonClickedEvent SettingsClickEvent => SettingsButton.onClick;
		public Button.ButtonClickedEvent ExitClickEvent => ExitButton.onClick;
	}
}
