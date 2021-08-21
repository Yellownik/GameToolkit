using UnityEngine;
using UnityEngine.UI;

namespace UIWrappers
{
	[RequireComponent(typeof(Image))]
	public class ImageWrapper : MonoBehaviour
	{
		public Image Image { get; private set; }

		private void Awake()
		{
			Image = GetComponent<Image>();
		}
	}
}
