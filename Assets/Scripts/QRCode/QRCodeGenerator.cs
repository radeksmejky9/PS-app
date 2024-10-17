using UnityEngine;
using ZXing;
using ZXing.QrCode;
using System.IO;
using Newtonsoft.Json;
using DocumentFormat.OpenXml.Wordprocessing;

[CreateAssetMenu(fileName = "NewQRCodeGenerator", menuName = "Utilities/QRCode Generator")]
public class QRCodeGenerator : ScriptableObject
{
    public SnappingPoint sp;

    public string jsonString = @"{
        ""Building"": ""DCUK"",
        ""Room"": ""Mistnost"",
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
        ""FBX"": {
            ""url"": ""https://github.com/radeksmejky9/PS/tree/main/Assets/Models/DCUK.fbx"",
        }
    }";

    public string building = "DCUK";
    public string room = "mistnost";
    public float positionX = 0.0f;
    public float positionY = 0.0f;
    public float positionZ = 0.0f;
    public float rotationX = 0.0f;
    public float rotationY = 0.0f;
    public float rotationZ = 0.0f;

    public string url = "https://github.com/radeksmejky9/PS/tree/main/Assets/Models/DCUK.fbx";


    public Texture2D qrCodeTexture;

    public void UpdateJSONFromFields()
    {
        sp = new SnappingPoint(
            building,
            room,
            new PositionData(positionX, positionY, positionZ),
            new RotationData(rotationX, rotationY, rotationZ),
            url);

        jsonString = JsonConvert.SerializeObject(sp, Formatting.Indented);

    }

    public void GenerateQR()
    {
        qrCodeTexture = GenerateQRFromJSON(JsonConvert.SerializeObject(sp));
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
    private Texture2D GenerateQRFromJSON(string jsonString, int width = 2048, int height = 2048)
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