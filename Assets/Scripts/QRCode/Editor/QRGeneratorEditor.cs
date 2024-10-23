using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(QRGenerator))]
public class QRGeneratorEditor : Editor
{
    private bool useJsonEditor = false;
    private Vector2 scrollPosition;
    private QRGenerator qrGenerator;

    public override void OnInspectorGUI()
    {
        qrGenerator = (QRGenerator)target;
        useJsonEditor = EditorGUILayout.Toggle("Edit JSON", useJsonEditor);

        if (useJsonEditor)
            UseJSONEditor();
        else
            UseFields();

        if (qrGenerator.QRCodeTexture != null)
            DisplayQRCode();

        EditorUtility.SetDirty(target);
    }

    private void UseFields()
    {
        SerializedProperty snappingPointProperty = serializedObject.FindProperty("SnappingPoint");
        qrGenerator.UpdateFieldsFromJSON();
        serializedObject.Update();

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(snappingPointProperty, new GUIContent("Snapping Point"));

        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
            qrGenerator.UpdateJSONFromFields();
        }

        if (GUILayout.Button("Generate QR Code"))
            qrGenerator.GenerateQR();
    }
    private void UseJSONEditor()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(300));
        qrGenerator.JsonString = EditorGUILayout.TextArea(qrGenerator.JsonString, GUILayout.ExpandHeight(true));
        EditorGUILayout.EndScrollView();

        if (GUILayout.Button("Save JSON"))
        {
            qrGenerator.UpdateFieldsFromJSON();
            useJsonEditor = false;
        }
    }

    private void DisplayQRCode()
    {
        GUILayout.Label("QR Code Preview:");
        GUILayout.Label(new GUIContent(qrGenerator.QRCodeTexture), GUILayout.Width(256), GUILayout.Height(256));

        if (GUILayout.Button("Save QR Code to PC"))
        {
            string name = qrGenerator.name.Replace(" ", "_");
            string path = EditorUtility.SaveFilePanel("Save QR Code", "", $"QR-{name}.png", "png");
            if (!string.IsNullOrEmpty(path))
            {
                qrGenerator.SaveQRCode(path);
            }
        }
    }
}
