using ButtonListeners;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ChangeStateElement))]
public class ChangeStateElementInspector : Editor
{
    private ChangeStateElement element;

    public override void OnInspectorGUI()
    {
        element = target as ChangeStateElement;
        EditorGUI.BeginChangeCheck();

        var data = element.StateData;
        data = DrawDataFields(data);

        if (EditorGUI.EndChangeCheck())
            element.StateData = data;
    }

    protected ChangeStateElement.Data DrawDataFields(ChangeStateElement.Data data)
    {
        GUILayout.BeginHorizontal();
        {
            data.EventToReceive = (ChangeStateEvent)EditorGUILayout.EnumPopup(data.EventToReceive);
            GUILayout.FlexibleSpace();
            data.ChangeStateType = (ChangeStateType)EditorGUILayout.EnumPopup(data.ChangeStateType);
        }
        GUILayout.EndHorizontal();

        switch (data.ChangeStateType)
        {
            case ChangeStateType.LocalScale:
                data.Scale = EditorGUILayout.Vector3Field("ScaleMult", data.Scale);
                data.DurationAnim = EditorGUILayout.FloatField("Tween time: ", data.DurationAnim);
                break;
            case ChangeStateType.LocalPosition:
                data.Position = EditorGUILayout.Vector3Field("Position Shift", data.Position);
                break;

            case ChangeStateType.ColorTint:
                data.ColorTint = EditorGUILayout.FloatField("Color Tint: ", data.ColorTint);
                break;
            case ChangeStateType.Color:
                data.Color = EditorGUILayout.ColorField("Color", data.Color);
                break;
            case ChangeStateType.Sprite:
                data.Sprite = (Sprite)EditorGUILayout.ObjectField("Sprite", data.Sprite, typeof(Sprite), false);
                break;

            case ChangeStateType.OnOff:
                data.IsActive = EditorGUILayout.Toggle("Active On State", data.IsActive);
                break;
        }

        return data;
    }
}

