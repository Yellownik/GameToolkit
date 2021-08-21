using UnityEngine;

namespace UIWrappers
{
	[RequireComponent(typeof(SpriteRenderer))]
	public class SpriteRendererWrapper : MonoBehaviour
	{
		public SpriteRenderer SpriteRenderer { get; private set; }

		private void Awake()
		{
			SpriteRenderer = GetComponent<SpriteRenderer>();
		}
	}
}
