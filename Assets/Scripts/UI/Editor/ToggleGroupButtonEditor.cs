using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

[CustomEditor(typeof(ToggleGroupButton))]
public class ToggleGroupButtonEditor : ToggleEditor
{
    SerializedProperty Label;
    SerializedProperty Content;
    protected override void OnEnable()
    {
        base.OnEnable();
        Label = serializedObject.FindProperty("Label");
        Content = serializedObject.FindProperty("Content");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();
        EditorGUILayout.PropertyField(Label, new GUIContent("Label"));
        EditorGUILayout.PropertyField(Content, new GUIContent("Content"));
        serializedObject.ApplyModifiedProperties();
    }
}