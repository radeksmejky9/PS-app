using UnityEngine;
using ZXing;
using ZXing.QrCode;
using System.IO;
using static UnityEditor.FilePathAttribute;

[CreateAssetMenu(fileName = "NewQRCodeGenerator", menuName = "Utilities/QRCode Generator")]
public class QRCodeGenerator : ScriptableObject
{
    // Prefilled JSON string
    [TextArea(5, 13)]
    public string jsonString = @"{
      ""Name"": ""mistnost"",
      ""Position"": {
        ""X"": 0.0,
        ""Y"": 0.0,
        ""Z"": 0.0
      },
      ""Rotation"": {
        ""X"": 0.0,
        ""Y"": 0.0,
        ""Z"": 0.0
      }
    }";

    public string objectName = "zasedacka";
    public float positionX = -3.5f;
    public float positionY = -0.15f;
    public float positionZ = 2.75f;
    public float rotationX = 0.0f;
    public float rotationY = 0.0f;
    public float rotationZ = 0.0f;

    public Texture2D qrCodeTexture;

    public void UpdateJSONFromFields()
    {
        jsonString = $@"{{
        ""Name"": ""{objectName}"",
        ""Position"": {{""X"": {positionX}, ""Y"": {positionY}, ""Z"": {positionZ}}},
        ""Rotation"": {{ ""X"": {rotationX}, ""Y"": {rotationY}, ""Z"": {rotationZ}}}}}";
    }

    public void GenerateQR()
    {
        qrCodeTexture = GenerateQRFromJSON(jsonString);
    }

    public void SaveQRCode(string path)
    {
        if (qrCodeTexture == null)
        {
            Debug.LogError("QR code texture is null! Generate a QR code first.");
            return;
        }

        byte[] bytes = qrCodeTexture.EncodeToPNG();
        File.WriteAllBytes(path, bytes);
        Debug.Log($"QR Code saved to: {path}");
    }

    private Texture2D GenerateQRFromJSON(string jsonString, int width = 256, int height = 256)
    {
        BarcodeWriter writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                Width = width,
                Height = height,
                Margin = 1
            }
        };

        Color32[] qrCodePixels = writer.Write(jsonString);
        Texture2D qrCodeTexture = new Texture2D(width, height);
        qrCodeTexture.SetPixels32(qrCodePixels);
        qrCodeTexture.Apply();

        return qrCodeTexture;
    }
}