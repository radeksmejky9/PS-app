using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

[CustomEditor(typeof(ToggleButton))]
public class ToggleButtonEditor : ToggleEditor
{
    SerializedProperty Label;
    SerializedProperty img;

    protected override void OnEnable()
    {
        base.OnEnable();
        Label = serializedObject.FindProperty("Label");
        img = serializedObject.FindProperty("img");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();
        EditorGUILayout.PropertyField(Label, new GUIContent("Label"));
        EditorGUILayout.PropertyField(img, new GUIContent("img"));
        serializedObject.ApplyModifiedProperties();
    }
}