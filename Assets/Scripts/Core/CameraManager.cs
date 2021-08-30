using UnityEngine;

namespace Core
{
	public class CameraManager : MonoBehaviour
	{
		public Camera Camera { get; private set; }

		private void Awake()
		{
			Camera = Camera.main;
		}
	}
}
