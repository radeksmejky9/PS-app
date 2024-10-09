using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(QRCodeGenerator))]
public class QRCodeGeneratorEditor : Editor
{
    private bool useJsonEditor = false;
    private Vector2 scrollPosition;
    private QRCodeGenerator qrGenerator;

    public override void OnInspectorGUI()
    {
        qrGenerator = (QRCodeGenerator)target;
        useJsonEditor = EditorGUILayout.Toggle("Edit JSON", useJsonEditor);

        if (useJsonEditor)
            OnUseJSONEditor();
        else
            OnUseFields();


        if (GUILayout.Button("Generate QR Code"))
            qrGenerator.GenerateQR();


        if (qrGenerator.qrCodeTexture != null)
            DisplayQRCode();

        EditorUtility.SetDirty(target);
    }

    private void OnUseFields()
    {
        EditorGUI.BeginChangeCheck();

        qrGenerator.objectName = EditorGUILayout.TextField("Name", qrGenerator.objectName);

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Position", EditorStyles.boldLabel);
        GUILayout.FlexibleSpace();
        GUILayout.Label("X", GUILayout.Width(10));
        qrGenerator.positionX = EditorGUILayout.FloatField(qrGenerator.positionX, GUILayout.Width(50));
        GUILayout.Label("Y", GUILayout.Width(10));
        qrGenerator.positionY = EditorGUILayout.FloatField(qrGenerator.positionY, GUILayout.Width(50));
        GUILayout.Label("Z", GUILayout.Width(10));
        qrGenerator.positionZ = EditorGUILayout.FloatField(qrGenerator.positionZ, GUILayout.Width(50));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Rotation", EditorStyles.boldLabel);
        GUILayout.FlexibleSpace();
        GUILayout.Label("X", GUILayout.Width(10));
        qrGenerator.rotationX = EditorGUILayout.FloatField(qrGenerator.rotationX, GUILayout.Width(50));
        GUILayout.Label("Y", GUILayout.Width(10));
        qrGenerator.rotationY = EditorGUILayout.FloatField(qrGenerator.rotationY, GUILayout.Width(50));
        GUILayout.Label("Z", GUILayout.Width(10));
        qrGenerator.rotationZ = EditorGUILayout.FloatField(qrGenerator.rotationZ, GUILayout.Width(50));
        EditorGUILayout.EndHorizontal();

        if (EditorGUI.EndChangeCheck())
            qrGenerator.UpdateJSONFromFields();

    }

    private void OnUseJSONEditor()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(150));
        qrGenerator.jsonString = EditorGUILayout.TextArea(qrGenerator.jsonString, GUILayout.Height(150));
        EditorGUILayout.EndScrollView();
    }

    private void DisplayQRCode()
    {
        GUILayout.Label("QR Code Preview:");
        GUILayout.Label(new GUIContent(qrGenerator.qrCodeTexture), GUILayout.Width(256), GUILayout.Height(256));

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
