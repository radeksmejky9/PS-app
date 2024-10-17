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
        qrGenerator.building = EditorGUILayout.TextField("Building", qrGenerator.building);
        qrGenerator.room = EditorGUILayout.TextField("Room", qrGenerator.room);

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Position", EditorStyles.boldLabel);
        GUILayout.FlexibleSpace();
        qrGenerator.positionX = DraggableFloatField("X", qrGenerator.positionX);
        qrGenerator.positionY = DraggableFloatField("Y", qrGenerator.positionY);
        qrGenerator.positionZ = DraggableFloatField("Z", qrGenerator.positionZ);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Rotation", EditorStyles.boldLabel);
        GUILayout.FlexibleSpace();
        qrGenerator.rotationX = DraggableFloatField("X", qrGenerator.rotationX);
        qrGenerator.rotationY = DraggableFloatField("Y", qrGenerator.rotationY);
        qrGenerator.rotationZ = DraggableFloatField("Z", qrGenerator.rotationZ);
        EditorGUILayout.EndHorizontal();
        qrGenerator.url = EditorGUILayout.TextField("Url", qrGenerator.url);


        if (EditorGUI.EndChangeCheck())
            qrGenerator.UpdateJSONFromFields();
    }

    private float DraggableFloatField(string label, float value)
    {
        EditorGUIUtility.labelWidth = 10;
        value = EditorGUILayout.FloatField(label, value, GUILayout.Width(60));
        return value;
    }



    private void OnUseJSONEditor()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(500));
        qrGenerator.jsonString = EditorGUILayout.TextArea(qrGenerator.jsonString, GUILayout.Height(500));
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
