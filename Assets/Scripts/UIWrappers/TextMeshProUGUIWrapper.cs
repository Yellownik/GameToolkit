using TMPro;
using UnityEngine;

namespace UIWrappers
{
	[RequireComponent(typeof(TextMeshProUGUI))]
	public class TextMeshProUGUIWrapper : MonoBehaviour
	{
		public TextMeshProUGUI TextMeshProUGUI { get; private set; }

		private void Awake()
		{
			TextMeshProUGUI = GetComponent<TextMeshProUGUI>();
		}
	}
}
