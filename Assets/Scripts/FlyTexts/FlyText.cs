using DG.Tweening;
using Orbox.Async;
using TMPro;
using UnityEngine;

namespace FlyTexts
{
	[RequireComponent(typeof(Canvas))]
	public class FlyText : MonoBehaviour
	{
		[SerializeField] private Transform Container;
		[SerializeField] private CanvasGroup CanvasGroup;
		[SerializeField] private TextMeshProUGUI TextLabel;

		[Space]
		[SerializeField] private Vector3 LocalOffset = new Vector3(0, 50, 0);

		private Canvas Canvas;

		private Sequence Seq;

		private void Awake()
		{
			Canvas = GetComponent<Canvas>();
		}

		#region Setters
		public void SetText(string text)
		{
			TextLabel.text = text;
		}

		public void SetWorldPosition(Vector3 position)
		{
			transform.position = position;
		}

		public void SetScale(Vector3 scale)
		{
			Container.localScale = scale;
		}

		public void SetSortingOrder(int order)
		{
			Canvas.overrideSorting = true;
			Canvas.sortingOrder = order;
		}
		#endregion

		public IPromise StartFly(float time)
		{
			if (Seq != null)
				Seq.Kill(true);

			Seq = DOTween.Sequence();

			Seq.Join(CanvasGroup
					.DOFade(1, time * 0.2f)
					.From(0)
					.SetEase(Ease.Linear));

			Seq.AppendInterval(time * 0.4f);

			Seq.Join(CanvasGroup
					.DOFade(0, time * 0.4f)
					.SetEase(Ease.Linear));

			Seq.Insert(0, Container
							.DOLocalMove(LocalOffset, time)
							.From(Vector3.zero)
							.SetEase(Ease.OutExpo));

			var promise = new Promise();
			Seq.OnComplete(() => promise.Resolve());

			return promise;
		}

		public void Interrupt()
		{
			if (Seq != null)
			{
				Seq.Kill(true);
				Seq = null;
			}

			gameObject.SetActive(false);
		}
	}
}
