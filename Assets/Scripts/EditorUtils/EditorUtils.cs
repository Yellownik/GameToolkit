using UnityEditor;
using UnityEngine;

namespace EditorUtils
{
    public class EditorUtils
    {
		public static void DrawScriptHeader<T>(T testUtils) where T : MonoBehaviour
		{
			GUI.enabled = false;
			EditorGUILayout.ObjectField("Script:", MonoScript.FromMonoBehaviour(testUtils), testUtils.GetType(), false);
			GUI.enabled = true;
		}
	}
}
