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

        for (int i = 0; i < element.dataList.Count; i++)
        {
            GUILayout.Space(10);
            EditorGUI.BeginChangeCheck();

            var data = element.dataList[i];
            data = DrawDataFields(data);

            if (EditorGUI.EndChangeCheck())
                element.dataList[i] = data;

        }
        DrawAddRemoveButtons(element);
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

    private void DrawAddRemoveButtons(ChangeStateElement element)
    {
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        {
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Add", GUILayout.Width(60)))
                element.CreateData_Editor();

            GUILayout.Space(30);
            if (element.dataList.Count > 0 && GUILayout.Button("Remove", GUILayout.Width(60)))
                element.RemoveData_Editor();

            GUILayout.FlexibleSpace();
        }
        GUILayout.EndHorizontal();
    }
}

