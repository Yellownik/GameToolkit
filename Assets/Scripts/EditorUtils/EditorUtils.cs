using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace EditorUtils
{
    public class EditorUtils
    {
	    #if UNITY_EDITOR
		public static void DrawScriptHeader<T>(T testUtils) where T : MonoBehaviour
		{
			GUI.enabled = false;
			EditorGUILayout.ObjectField("Script:", MonoScript.FromMonoBehaviour(testUtils), testUtils.GetType(), false);
			GUI.enabled = true;
		}
		#endif
	}
}
