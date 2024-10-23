using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;

[CustomEditor(typeof(BeanQRGenerator))]
public class BeanQRGeneratorEditor : Editor
{
    private BeanQRGenerator bean;
    private bool init = false;
    public override void OnInspectorGUI()
    {
        bean = (BeanQRGenerator)target;
        base.OnInspectorGUI();

        if (GUILayout.Button("Generate QR Code"))
        {
            bean.InitGenerator();
            init = true;
        }

        if (init && bean.QRGenerator.QRCodeTexture != null)
            DisplayQRCode();
    }

    private void DisplayQRCode()
    {
        GUILayout.Label("QR Code Preview:");
        GUILayout.Label(new GUIContent(bean.QRGenerator.QRCodeTexture), GUILayout.Width(256), GUILayout.Height(256));
        string name = $"{bean.QRGenerator.SnappingPoint.Building} - {bean.QRGenerator.SnappingPoint.Room}";

        if (GUILayout.Button("Save QR Code to PC"))
        {
            string path = EditorUtility.SaveFilePanel("Save QR Code", "", $"QR-{name}.png", "png");
            if (!string.IsNullOrEmpty(path))
            {
                bean.QRGenerator.SaveQRCode(path);
            }
        }
        if (GUILayout.Button("Save QR Code to Snapping Points Folder"))
        {
            AssetDatabase.CreateAsset(bean.QRGenerator, $"Assets/ScriptableObjects/SnappingPoints/{name}.asset");
            AssetDatabase.SaveAssets();
        }
    }
}
