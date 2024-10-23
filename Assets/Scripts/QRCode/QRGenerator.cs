using UnityEngine;
using ZXing;
using ZXing.QrCode;
using System.IO;
using Newtonsoft.Json;
using System;
using System.Dynamic;

[CreateAssetMenu(fileName = "NewQRCodeGenerator", menuName = "Utilities/QRCode Generator")]
public class QRGenerator : ScriptableObject
{
    public SnappingPoint SnappingPoint;
    public Texture2D QRCodeTexture;

    public string JsonString = @"{
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
        },
        ""url"": ""https://github.com/radeksmejky9/PS/tree/main/Assets/Models/DCUK.fbx""
    }";

    public void Awake()
    {
        SnappingPoint = new SnappingPoint("DCUK", "Mistnost", new PositionData(0, 0, 0), new RotationData(0, 0, 0), "");
    }
    public void UpdateJSONFromFields()
    {

        try
        {
            JsonString = JsonConvert.SerializeObject(SnappingPoint, Formatting.Indented);
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to convert." + e.Message);
        }
    }

    public void UpdateFieldsFromJSON()
    {
        try
        {
            SnappingPoint = JsonConvert.DeserializeObject<SnappingPoint>(JsonString);
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to convert." + e.Message);
        }

    }

    public void GenerateQR()
    {
        QRCodeTexture = GenerateQRFromJSON(JsonConvert.SerializeObject(SnappingPoint, Formatting.None));
    }

    public void SaveQRCode(string path)
    {
        if (QRCodeTexture == null)
        {
            Debug.LogError("QR code texture is null! Generate a QR code first.");
            return;
        }

        byte[] bytes = QRCodeTexture.EncodeToPNG();
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

        Color32[] qrCodePixels = writer.Write(jsonString.RemoveWhiteSpace().Base64Encode());
        Texture2D qrCodeTexture = new Texture2D(width, height);
        qrCodeTexture.SetPixels32(qrCodePixels);
        qrCodeTexture.Apply();
        return qrCodeTexture;
    }

    public static QRGenerator CreateInstance(SnappingPoint sp)
    {
        var qrgen = ScriptableObject.CreateInstance<QRGenerator>();
        qrgen.SnappingPoint = sp;
        qrgen.UpdateJSONFromFields();
        return qrgen;
    }
}