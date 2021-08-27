using DG.Tweening;
using Orbox.Async;
using UnityEngine;

namespace FlyTexts
{
	public abstract class AbstractFlyText : MonoBehaviour
	{
		private const float FADE_DELAY = 1.6f;
		private const float FADE_TIME = 0.6f;
		private const float POS_Y_SHIFT = 70f;

		private readonly Color StartColor = Color.white;
		private readonly Color EndColor = new Color(1, 1, 1, 0);

		private Tweener PosTween;
		private Tweener ColorTween;

		public abstract void SetText(string text);
		public abstract Color TextColor { protected set; get; }
		public abstract Vector3 LabelPosition { protected set; get; }

		public IPromise StartFly(float time)
		{
			var promise = new Promise();

			Vector3 endPos = LabelPosition + new Vector3(0, POS_Y_SHIFT, 0);
			PosTween = DOTween.To(() => LabelPosition, pos => LabelPosition = pos, endPos, time)
				.SetEase(Ease.OutExpo)
				.OnComplete(() =>
				{
					promise.Resolve();
				});

			TextColor = StartColor;
			ColorTween = DOTween.To(() => TextColor, col => TextColor = col, EndColor, FADE_TIME)
				.SetDelay(FADE_DELAY)
				.SetEase(Ease.Linear);

			return promise;
		}

		public void Cancel()
		{
			if (ColorTween != null)
			{
				ColorTween.Kill(true);
				ColorTween = null;
			}

			if (PosTween != null)
			{
				PosTween.Kill(true);
				PosTween = null;
			}
		}

		private void OnDisable()
		{
			if (PosTween != null || ColorTween != null)
			{
				Destroy(gameObject);
			}
		}
	}
}
