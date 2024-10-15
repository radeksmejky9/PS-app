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

        // Position - X, Y, Z in one row
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Position", EditorStyles.boldLabel);
        GUILayout.FlexibleSpace();
        qrGenerator.positionX = DraggableFloatField("X", qrGenerator.positionX);
        qrGenerator.positionY = DraggableFloatField("Y", qrGenerator.positionY);
        qrGenerator.positionZ = DraggableFloatField("Z", qrGenerator.positionZ);
        EditorGUILayout.EndHorizontal();

        // Rotation - X, Y, Z in one row
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Rotation", EditorStyles.boldLabel);
        GUILayout.FlexibleSpace();
        qrGenerator.rotationX = DraggableFloatField("X", qrGenerator.rotationX);
        qrGenerator.rotationY = DraggableFloatField("Y", qrGenerator.rotationY);
        qrGenerator.rotationZ = DraggableFloatField("Z", qrGenerator.rotationZ);
        EditorGUILayout.EndHorizontal();

        if (EditorGUI.EndChangeCheck())
            qrGenerator.UpdateJSONFromFields();
    }

    // Helper function to create aligned draggable float fields in one row
    private float DraggableFloatField(string label, float value)
    {
        EditorGUIUtility.labelWidth = 10;  // Adjust the label width to fit
        value = EditorGUILayout.FloatField(label, value, GUILayout.Width(60));  // Make sure each float field has the same width for alignment
        return value;
    }



    private void OnUseJSONEditor()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(200));
        qrGenerator.jsonString = EditorGUILayout.TextArea(qrGenerator.jsonString, GUILayout.Height(200));
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
