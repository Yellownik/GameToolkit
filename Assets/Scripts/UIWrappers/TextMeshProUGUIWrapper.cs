using Orbox.Async;
using TMPro;
using UnityEngine;
using Utils;

namespace UIWrappers
{
	[RequireComponent(typeof(TextMeshProUGUI))]
	public class TextMeshProUGUIWrapper : BaseUIWrapper
	{
		public TextMeshProUGUI TextMeshProUGUI { get; private set; }

		private void Awake()
		{
			TextMeshProUGUI = GetComponent<TextMeshProUGUI>();
		}

		public override IPromise Show()
		{
			var promise = BaseScale(true);
			transform.DoMove(true);
			TextMeshProUGUI.DoFade(true);

			return promise;
		}

		public override IPromise Hide()
		{
			var promise = BaseScale(false);
			transform.DoMove(false);
			TextMeshProUGUI.DoFade(false);

			return promise;
		}
	}
}
