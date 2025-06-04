using AudioSources;
using Core;
using GameManagers;
using Orbox.Async;
using Saving;

namespace UI.Menus
{
	public class MenuManager
	{
		private readonly InputManager InputManager;
		private readonly FadeManager FadeManager;
		private readonly AudioManager AudioManager;

		private MainMenuView MainMenu;
		private PauseMenuView PauseMenu;
		private TitresMenuView TitresMenu;

		private Promise PlayPromise = new Promise();
		private Promise ExitPromise = new Promise();
		
		private bool IsGameStarted = false;
		private bool IsInputActive;

		public MenuManager(ViewFactory viewFactory, InputManager inputManager, FadeManager fadeManager, 
			ISaveManager saveManager, AudioManager audioManager)
		{
			FadeManager = fadeManager;
			InputManager = inputManager;
			AudioManager = audioManager;

			InputManager.GamePausing += SwitchPauseMenu;

			InitMenus(viewFactory);
			PauseMenu.Init(saveManager, audioManager.SoundManager);
		}

		private void InitMenus(ViewFactory viewFactory)
		{
			MainMenu = viewFactory.CreateMainMenuView();
			MainMenu.PlayClicked += Play;
			MainMenu.SettingsClicked += SwitchPauseMenu;
			MainMenu.ExitClicked += Exit;
			MainMenu.Disable();

			PauseMenu = viewFactory.CreatePauseMenuView();
			PauseMenu.ReturnClicked += HidePauseMenu;
			PauseMenu.ExitClicked += Exit;
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
			return PlayPromise;
		}

		public IPromise WaitForExit()
		{
			return ExitPromise;
		}
		
		private void Play()
		{
			if (FadeManager.IsFading)
				return;

			var promise = PlayPromise;
			PlayPromise = new Promise();

			MainMenu.Hide();
			FadeManager.FadeOut()
				.Done(() =>
				{
					IsGameStarted = true;
					promise.Resolve();
				});
		}
		#endregion

		#region PauseMenu
		private void SwitchPauseMenu()
		{
			if (FadeManager.IsFading || PauseMenu.IsAnimating)
			{
				return;
			}

			if (PauseMenu.IsShown)
			{
				HidePauseMenu();
				return;
			}
			
			IsInputActive = InputManager.IsActive;

			PauseMenu.ShowSettingsOnly(!IsGameStarted);
			PauseMenu.Show();
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
			TitresMenu.ShowCounter();
		}

		private void Exit()
		{
			if (FadeManager.IsFading)
				return;

			AudioManager.StopMusic();
			
			FadeManager.ResetFadeCenter();
			FadeManager.FadeOut()
				.Done(() => ExitPromise.Resolve());
		}
	}
}
