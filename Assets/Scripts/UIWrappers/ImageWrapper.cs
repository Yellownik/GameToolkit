using Orbox.Async;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UIWrappers
{
	[RequireComponent(typeof(Image))]
	public class ImageWrapper : BaseUIWrapper
	{
		public Image Image { get; private set; }

		private void Awake()
		{
			Image = GetComponent<Image>();
		}

		public override IPromise Show()
		{
			var promise = BaseScale(true);
			transform.DoMove(true);
			Image.DoFade(true);

			return promise;
		}

		public override IPromise Hide()
		{
			var promise = BaseScale(false);
			transform.DoMove(false);
			Image.DoFade(false);

			return promise;
		}
	}
}
