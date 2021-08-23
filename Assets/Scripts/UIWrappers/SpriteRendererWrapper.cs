using Orbox.Async;
using UnityEngine;
using Utils;

namespace UIWrappers
{
	[RequireComponent(typeof(SpriteRenderer))]
	public class SpriteRendererWrapper : BaseUIWrapper
	{
		public SpriteRenderer SpriteRenderer { get; private set; }

		private void Awake()
		{
			SpriteRenderer = GetComponent<SpriteRenderer>();
		}

		public override IPromise Show()
		{
			var promise = BaseScale(true);
			transform.DoMove(true);
			SpriteRenderer.DoFade(true);

			return promise;
		}

		public override IPromise Hide()
		{
			var promise = BaseScale(false);
			transform.DoMove(false);
			SpriteRenderer.DoFade(false);

			return promise;
		}
	}
}
