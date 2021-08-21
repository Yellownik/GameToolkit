using ButtonListeners;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ChangeStateElement))]
public class ChangeStateElementInspector : Editor
{
    public override void OnInspectorGUI()
    {
        ChangeStateElement elem = target as ChangeStateElement;

        GUILayout.BeginHorizontal();
        {
            elem.EventToReceive = (ChangeStateEvent)EditorGUILayout.EnumPopup(elem.EventToReceive);
            GUILayout.Space(10);
            elem.ChangeStateType = (ChangeStateType)EditorGUILayout.EnumPopup(elem.ChangeStateType);
        }
        GUILayout.EndHorizontal();

        switch (elem.ChangeStateType)
        {
            case ChangeStateType.Color:
                elem.NormalColor = EditorGUILayout.ColorField("Normal Color", elem.NormalColor);
                elem.StateColor = EditorGUILayout.ColorField("State Color", elem.StateColor);

                break;

            case ChangeStateType.ColorTint:
                elem.StateColorTint = EditorGUILayout.FloatField("Color Tint: ", elem.StateColorTint);
                break;

            case ChangeStateType.LocalPosition:
                elem.StatePositionShift = EditorGUILayout.Vector3Field("PositionShift", elem.StatePositionShift);
                break;

            case ChangeStateType.LocalScale:
                elem.StateScaleMult = EditorGUILayout.Vector3Field("ScaleMult", elem.StateScaleMult);
                break;

            case ChangeStateType.OnOff:
                DrawOnOffToggles(elem);
                break;

            case ChangeStateType.Image:
                elem.StateSprite = (Sprite) EditorGUILayout.ObjectField("Sprite", elem.StateSprite, typeof(Sprite), false);
                break;
        }
    }

    private void DrawOnOffToggles(ChangeStateElement elem)
    {
        elem.StateActive = EditorGUILayout.Toggle("Active On State", elem.StateActive);
    }
}

