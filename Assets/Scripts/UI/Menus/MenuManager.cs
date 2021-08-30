using AudioSources;
using Core;
using Orbox.Async;
using Saving;

namespace UI.Menus
{
	public class MenuManager
	{
		private InputManager InputManager;
		private FadeManager FadeManager;

		private MainMenuView MainMenu;
		private PauseMenuView PauseMenu;
		private TitresMenuView TitresMenu;

		private Promise PlayDeferred = new Promise();
		private bool IsGameStarted = false;
		private bool IsInputActive;

		public MenuManager(ViewFactory viewFactory, InputManager inputManager, FadeManager fadeManager, 
			ISaveManager saveManager, AudioManager audioManager)
		{
			FadeManager = fadeManager;
			InputManager = inputManager;

			InputManager.GamePausing += SwitchPauseMenu;

			InitMenus(viewFactory);
			PauseMenu.Init(saveManager, audioManager.SoundManager);
		}

		private void InitMenus(ViewFactory viewFactory)
		{
			MainMenu = viewFactory.CreateMainMenuView();
			MainMenu.PlayClicked += StartTheGame;
			MainMenu.SettingsClicked += SwitchPauseMenu;
			MainMenu.ExitClicked += ExitGame;
			MainMenu.Disable();

			PauseMenu = viewFactory.CreatePauseMenuView();
			PauseMenu.ReturnClicked += HidePauseMenu;
			PauseMenu.ExitClicked += ExitGame;
			PauseMenu.Disable();

			TitresMenu = viewFactory.CreateTitresMenuView();
			TitresMenu.Disable();
		}

		#region MainMenu
		public void ShowMainMenu()
		{
			MainMenu.Show();
		}

		public IPromise WaitForPlay()
		{
			return PlayDeferred;
		}

		private void StartTheGame()
		{
			if (FadeManager.IsFading)
				return;

			var promise = PlayDeferred;
			PlayDeferred = new Promise();

			FadeManager.FadeOut()
				.Done(() =>
				{
					MainMenu.Hide();
					IsGameStarted = true;
					promise.Resolve();
				});
		}
		#endregion

		#region PauseMenu
		private void SwitchPauseMenu()
		{
			if (FadeManager.IsFading || PauseMenu.IsAnimating)
				return;

			if (!PauseMenu.IsShown)
			{
				IsInputActive = InputManager.IsActive;
				InputManager.SetActiveState(false);

				PauseMenu.ShowSettingsOnly(!IsGameStarted);
				PauseMenu.Show();
			}
			else
			{
				HidePauseMenu();
			}
		}

		private void HidePauseMenu()
		{
			if (PauseMenu.IsAnimating || !PauseMenu.IsShown)
				return;

			PauseMenu.Hide()
				.Done(() => InputManager.SetActiveState(IsInputActive))
				.Done(() => PauseMenu.ShowSettingsOnly(false));
		}
		#endregion

		public void ShowTitresMenu()
		{
			TitresMenu.Show();
		}

		private void ExitGame()
		{
			if (FadeManager.IsFading)
				return;

			Root.AudioManager.StopMusic();
			FadeManager.ResetFadeCenter();
			FadeManager.FadeOut()
				.Done(() =>
				{
					throw new System.NotImplementedException();
					//Root.GetGameManager().ExitTheGame();
				});
		}
	}
}
