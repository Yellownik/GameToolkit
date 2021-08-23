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
		[SerializeField] private Button ReturnButton;
		[SerializeField] private Button ExitButton;

		[Space]
		[SerializeField] private float VolumeApplyTime = 0.3f;

		[Space]
		[SerializeField] private float AnimTime = 0.3f;
		[SerializeField] private float FadeFrom = 0.3f;
		[SerializeField] private float ScaleFrom = 0.8f;

		[Space]
		[SerializeField] private CanvasGroup CanvasGroup;
		[SerializeField] private List<Transform> TransformForScale;

		private ISoundManager SoundManager;
		//private SaveManager SaveManager;

		private Tweener VolumeApplyTweener;
		public event Action ReturnClicked = () => { };
		public event Action ExitClicked = () => { };

		public bool IsShown { get; private set; }
		public bool IsAnimating { get; private set; }

		private void Awake()
		{
			SoundManager = Root.AudioManager.SoundManager;
			//SaveManager = Root.SaveManager;
			TryExtractSavedData();

			foreach (var trans in TransformForScale)
				trans.localScale = Vector3.one * ScaleFrom;

			CanvasGroup.alpha = FadeFrom;
		}

		private void Start()
		{
			VolumeSlider.onValueChanged.AddListener(OnVolumeSliderValueChanged);
			ReturnButton.onClick.AddListener(OnReturnButtonClicked);
			ExitButton.onClick.AddListener(OnExitButtonClicked);
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

		#region BasseView
		public override void Disable()
		{
			IsShown = false;
			gameObject.SetActive(false);
		}

		public override void Enable()
		{
			IsShown = false;
			gameObject.SetActive(true);
		}

		public override IPromise Hide()
		{
			SaveSettings();

			var promise = ShowAnimated(false);
			promise.Done(() =>
			{
				IsShown = false;
				Disable();
			});
			return promise;
		}

		public override IPromise Show()
		{
			Enable();
			IsShown = true;

			var promise = ShowAnimated(true);
			return promise;
		}

		private IPromise ShowAnimated(bool isShow)
		{
			IsAnimating = true;
			var promise = new Promise();

			if (isShow)
			{
				foreach (var trans in TransformForScale)
					trans.DOScale(1, AnimTime);

				CanvasGroup.DOFade(1, AnimTime)
					.OnComplete(() => promise.Resolve());
			}
			else
			{
				foreach (var trans in TransformForScale)
					trans.DOScale(ScaleFrom, AnimTime);

				CanvasGroup.DOFade(FadeFrom, AnimTime)
					.OnComplete(() => promise.Resolve());
			}

			promise
				.Done(() => IsAnimating = false);
			return promise;
		}

		#endregion
	}
}
