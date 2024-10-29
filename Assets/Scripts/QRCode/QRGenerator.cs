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

        QRCodeTexture = GenerateQRFromJSON(SnappingPoint.Encode(SnappingPoint));
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
    private Texture2D GenerateQRFromJSON(string qrText, int width = 2048, int height = 2048)
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
        Texture2D qrCodeTexture = new Texture2D(width, height);
        qrCodeTexture.SetPixels32(qrCodePixels);
        qrCodeTexture.Apply();

        var font = Resources.Load("CustomFontArial") as Font;

        TextToTexture txt = new TextToTexture(font, 10, 10, Array.Empty<PerCharacterKerning>(), true);
        var textTure = txt.CreateTextToTexture("ahoj", 0, 0, 2048, 3, 0.75f);


        return AddWatermark(qrCodeTexture, textTure, 0, qrCodeTexture.height - textTure.height);
    }
    public static QRGenerator CreateInstance(SnappingPoint sp)
    {
        var qrgen = ScriptableObject.CreateInstance<QRGenerator>();
        qrgen.SnappingPoint = sp;
        qrgen.UpdateJSONFromFields();
        return qrgen;
    }

    public static Texture2D AddWatermark(Texture2D background, Texture2D watermark, int startX, int startY)
    {
        Texture2D newTex = new Texture2D(background.width, background.height, background.format, false);
        for (int x = 0; x < background.width; x++)
        {
            for (int y = 0; y < background.height; y++)
            {
                if (x >= startX && y >= startY && x < watermark.width && y < watermark.height)
                {
                    Color bgColor = background.GetPixel(x, y);
                    Color wmColor = watermark.GetPixel(x - startX, y - startY);

                    Color final_color = Color.Lerp(bgColor, wmColor, wmColor.a / 1.0f);

                    newTex.SetPixel(x, y, final_color);
                }
                else
                    newTex.SetPixel(x, y, background.GetPixel(x, y));
            }
        }

        newTex.Apply();
        return newTex;
    }


}