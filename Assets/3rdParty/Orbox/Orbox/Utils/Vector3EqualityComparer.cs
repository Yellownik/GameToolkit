using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Orbox.Utils
{
	// Used for Distionary comparetion, to avoid boxing
	public class Vector3EqualityComparer : IEqualityComparer<Vector3>
	{
		private const float RoundLevel = 10000f;
		
		public bool Equals( Vector3 v1, Vector3 v2 )
		{
			return v1 == v2;
		}
		
		public int GetHashCode( Vector3 v )
		{
			float x = Mathf.Round(v.x * RoundLevel) / RoundLevel;
			float y = Mathf.Round(v.y * RoundLevel) / RoundLevel;
			float z = Mathf.Round(v.z * RoundLevel) / RoundLevel;

			return x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode();
		}
		
	}
}