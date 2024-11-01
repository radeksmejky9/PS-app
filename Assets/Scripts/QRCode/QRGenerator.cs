using UnityEngine;
using ZXing;
using ZXing.QrCode;
using System.IO;
using Newtonsoft.Json;
using System;

[CreateAssetMenu(fileName = "NewQRCodeGenerator", menuName = "Utilities/QRCode Generator")]
public class QRGenerator : ScriptableObject
{
    public SnappingPoint SnappingPoint;
    public Texture2D QRCodeTexture;

    private Texture2D rawQRTexture;

    public string JsonString = @"{
    ""Building"": ""DCUK"",
    ""Room"": ""Mistnost"",
    ""Position"": {
        ""x"": 0.0,
        ""y"": 0.0,
        ""z"": 0.0
    },
    ""Rotation"": 0.0,
    ""Url"": ""https://github.com/radeksmejky9/PS/tree/main/Assets/Models/DCUK.fbx""
    }";

    private readonly JsonSerializerSettings settings = new JsonSerializerSettings
    {
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    };

    public void Awake()
    {
        SnappingPoint = new SnappingPoint("DCUK", "Mistnost", new Vector3(0, 0, 0), 0, "google.com");
        QRSpawner.Instance.OnQRGenerated += DisplayQR;
    }


    public void UpdateJSONFromFields()
    {
        try
        {
            JsonString = JsonConvert.SerializeObject(SnappingPoint, Formatting.Indented, settings);
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
            SnappingPoint = JsonConvert.DeserializeObject<SnappingPoint>(JsonString, settings);
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to convert: " + e.Message + "\n" + e.StackTrace);
        }
    }

    public void GenerateQR()
    {
        GenerateQRFromJSON(SnappingPoint.Encode(SnappingPoint));
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
    private void GenerateQRFromJSON(string qrText, int width = 2048, int height = 2048)
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
        Color32[] qrCodePixels = writer.Write(qrText.RemoveWhiteSpace().Base64Encode());
        Texture2D rawQRTexture = new Texture2D(width, height);
        rawQRTexture.SetPixels32(qrCodePixels);
        rawQRTexture.Apply();

        QRSpawner.Instance.GenerateTexture(rawQRTexture, $"{SnappingPoint.Building} - {SnappingPoint.Room}", (texture) =>
        {
            QRCodeTexture = texture;
        });
    }

    private void DisplayQR(Texture2D d)
    {
        throw new NotImplementedException();
    }


    public static QRGenerator CreateInstance(SnappingPoint sp)
    {
        var qrgen = ScriptableObject.CreateInstance<QRGenerator>();
        qrgen.SnappingPoint = sp;
        qrgen.UpdateJSONFromFields();
        return qrgen;
    }
}