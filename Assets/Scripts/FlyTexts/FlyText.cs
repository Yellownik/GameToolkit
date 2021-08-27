using TMPro;
using UnityEngine;

namespace FlyTexts
{
	[RequireComponent(typeof(Canvas))]
	public class FlyText : AbstractFlyText
	{
		[SerializeField] private TextMeshProUGUI _textLabel;
		[SerializeField] private CanvasGroup _canvasGroup;

		private Canvas _canvas;

		private Color _defaultColor = new Color(1, 1, 1, 1);

		public override Color TextColor
		{
			get
			{
				_defaultColor.a = _canvasGroup.alpha;
				return _defaultColor;
			}
			protected set => _canvasGroup.alpha = value.a;
		}

		public override Vector3 LabelPosition
		{
			get => _textLabel.transform.localPosition;
			protected set => _textLabel.transform.localPosition = value;
		}

		private void Awake()
		{
			_canvas = GetComponent<Canvas>();
		}

		public override void SetText(string text)
		{
			_textLabel.text = text;
		}

		public void SetWorldPosition(Vector3 position)
		{
			_textLabel.transform.position = position;
		}

		public void SetScale(Vector3 scale)
		{
			_textLabel.transform.localScale = scale;
		}

		public void SetSortingOrder(int order)
		{
			_canvas.overrideSorting = true;
			_canvas.sortingOrder = order;
		}
	}
}
