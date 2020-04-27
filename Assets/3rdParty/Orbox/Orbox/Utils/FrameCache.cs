using UnityEngine;
using System;
using System.Collections;

namespace Orbox.Utils
{

	public class FrameCache<T> where T: struct
	{

		private Func<T> GetValueFunc;
		private int CachedFrame = -1;
		private T CachedValue;

		public FrameCache(Func<T> getValueFunc)
		{
			GetValueFunc = getValueFunc;
		}
		
		public T Value 
		{ 
			get
			{
				int currentFrame = Time.frameCount;

				if (CachedFrame != currentFrame)
				{
					CachedFrame = currentFrame;
					CachedValue = GetValueFunc();
				}

				return CachedValue;
			}
		}
	}
}