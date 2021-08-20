using Orbox.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
	public class Root : MonoBehaviour
	{
		public static ITimers Timers { get; private set; }

		private static Transform RootTransform;

		private void Awake()
		{
			RootTransform = transform;

			Init();
		}

		private void Init()
		{
			Timers = MonoExtensions.MakeComponent<Timers>(RootTransform);
		}
	}
}
