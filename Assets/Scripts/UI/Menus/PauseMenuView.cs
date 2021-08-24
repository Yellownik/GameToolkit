using ButtonListeners;
using Core;
using DG.Tweening;
using Orbox.Async;
using Orbox.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menus
{
	public class PauseMenuView : BaseMenuView
	{
		[SerializeField] private Slider VolumeSlider;
		[SerializeField] private ButtonListener ReturnButton;
		[SerializeField] private ButtonListener ExitButton;

		[Space]
		[SerializeField] private float VolumeApplyTime = 0.3f;

		private ISoundManager SoundManager;
		//private SaveManager SaveManager;

		private Tweener VolumeApplyTweener;
		public event Action ReturnClicked = () => { };
		public event Action ExitClicked = () => { };

		private void Awake()
		{
			SoundManager = Root.AudioManager.SoundManager;
			//SaveManager = Root.SaveManager;
			TryExtractSavedData();
		}

		private void Start()
		{
			VolumeSlider.onValueChanged.AddListener(OnVolumeSliderValueChanged);
			ReturnButton.AddFunction(OnReturnButtonClicked);
			ExitButton.AddFunction(OnExitButtonClicked);
		}

		public void ShowSettingsOnly(bool isSettings)
		{
			ExitButton.gameObject.SetActive(!isSettings);
		}

		private void TryExtractSavedData()
		{
			/*if (SaveManager.HasSavedData)
			{
				var volume = SaveManager.SettingsData.Volume;
				VolumeSlider.value = volume;
				SoundManager.SetVolume(volume);
			}
			else
			{
				SoundManager.SetVolume(VolumeSlider.value);
			}*/
		}

		private void OnVolumeSliderValueChanged(float value)
		{
			if (VolumeApplyTweener != null)
				VolumeApplyTweener.Kill();

			float currentVolume = SoundManager.GetVolume();

			VolumeApplyTweener = DOTween.To(
				setter: v => SoundManager.SetVolume(v),
				startValue: currentVolume,
				endValue: value,
				duration: VolumeApplyTime)
				.SetEase(Ease.Linear);
		}

		private void OnReturnButtonClicked()
		{
			ReturnClicked();
		}

		private void OnExitButtonClicked()
		{
			SaveSettings();
			ExitClicked();
		}

		private void SaveSettings()
		{
			/*SaveManager.SetVolume(SoundManager.GetVolume());
			SaveManager.SaveData();*/
		}
	}
}
