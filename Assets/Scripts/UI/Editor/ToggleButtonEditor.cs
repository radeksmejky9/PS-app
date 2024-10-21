using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

[CustomEditor(typeof(ToggleButton))]
public class ToggleButtonEditor : ToggleEditor
{
    SerializedProperty Label;

    protected override void OnEnable()
    {
        base.OnEnable();
        Label = serializedObject.FindProperty("Label");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();
        EditorGUILayout.PropertyField(Label, new GUIContent("Label"));
        serializedObject.ApplyModifiedProperties();
    }
}