using AudioSources;
using Core;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Orbox.Async;
using Orbox.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AudioSources
{
	public class AudioManager : MonoBehaviour
	{
		[SerializeField] private float MaxVolume = 1f;
		[SerializeField] private float FadeTime = 0.5f;

		public ISoundManager SoundManager { get; private set; }
		private ITimerService Timers;

		private EMusic CurrentTheme;
		private Tween FadeUpTween = null;

		public void Init(ISoundManager soundManager, ITimerService timers)
		{
			SoundManager = soundManager;
			Timers = timers;
		}

		public void PlayEffect(ESounds eEffect)
		{
			SoundManager.Play(eEffect);
		}

		public void PlayTheme(EMusic newTheme, float delay = 0, float fadeTime = -1)
		{
			if (delay == 0)
				FadeUp(newTheme, fadeTime);
			else
				Timers.Wait(delay)
					.Done(() => FadeUp(newTheme, fadeTime));
		}

		public void StopTheme(float delay = 0, float fadeTime = -1)
		{
			if (delay == 0)
				FadeDown(fadeTime);
			else
				Timers.Wait(delay)
					.Done(() => FadeDown(fadeTime));
		}

		private void FadeDown(float fadeTime)
		{
			if (fadeTime < 0)
				fadeTime = FadeTime;

			fadeTime *= 0.95f;
			if (FadeUpTween != null)
			{
				FadeUpTween.Kill();
				FadeUpTween = null;
			}

			var theme = CurrentTheme;
			float volume = SoundManager.GetVolume(theme);

			DOTween.To(
					() => volume,
					x => SoundManager.SetVolume(theme, x),
					0,
					fadeTime)
				.OnComplete(() =>
				{
					SoundManager.Stop(theme);
				});
		}

		private void FadeUp(EMusic theme, float fadeTime)
		{
			if (fadeTime < 0)
				fadeTime = FadeTime;

			CurrentTheme = theme;

			var curTheme = CurrentTheme;
			SoundManager.Play(curTheme);

			float volume = 0;
			SoundManager.SetVolume(curTheme, volume);

			FadeUpTween = DOTween.To(
				() => volume,
				x => SoundManager.SetVolume(curTheme, x),
				MaxVolume,
				fadeTime);
		}
	}
}