using System.Collections;
using UnityEngine;
using ZXing;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using Unity.Collections;
using UnityEngine.XR.ARSubsystems;
using System;
using System.ComponentModel;
using TMPro;
using ZXing.QrCode.Internal;
using Newtonsoft.Json;

public class BarcodeScanner : MonoBehaviour
{
    public static Action<SnappingPoint> QRScanned;

    public ARCameraManager CameraManager;

    [Range(1, 1920)]
    public int previewWidth;
    [Range(1, 1080)]
    public int previewHeight;

    public bool ScanningMode
    {
        get { return _scanningMode; }
        private set
        {
            _qrCode = string.Empty;

            if (value == true)
            {
                InitScanningArea();
                StartCoroutine(GetQRCode());
            }

            _scanningMode = value;
        }
    }


    private bool _scanningMode = false;
    private string _qrCode = string.Empty;
    private RectInt _scanningArea;

    private int x = 0;
    private int y = 0;

    private void Start()
    {
        x = Screen.width / 2 - previewWidth / 2;
        y = Screen.height / 2 - previewHeight / 2;
    }

    public void SetScanningMode()
    {
        ScanningMode = !ScanningMode;
    }
    private IEnumerator GetQRCode()
    {
        IBarcodeReader barCodeReader = new BarcodeReader();
        while (string.IsNullOrEmpty(_qrCode))
        {
            if (CameraManager.TryAcquireLatestCpuImage(out XRCpuImage image))
            {
                var cameraImageTexture = GetImageTexture(image);
                try
                {
                    var Result = barCodeReader.Decode(cameraImageTexture.GetPixels32(), cameraImageTexture.width, cameraImageTexture.height);
                    if (Result != null)
                    {
                        _qrCode = Result.Text;
                        if (!string.IsNullOrEmpty(_qrCode))
                        {
                            QRScanned?.Invoke(JsonConvert.DeserializeObject<SnappingPoint>(_qrCode));
                            Debug.Log("DECODED TEXT FROM QR: " + _qrCode);
                            ScanningMode = false;
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("Failed to scan");
                    Debug.LogError(ex.Message);
                    _qrCode = string.Empty;
                }
                finally
                {
                    image.Dispose();
                }
            }
            yield return null;
        }
    }
    void OnGUI()
    {
        if (!ScanningMode) return;

        Texture2D backgroundTexture = new Texture2D(1, 1);
        backgroundTexture.SetPixel(0, 0, new Color(0, 0, 0, 0.5f));
        backgroundTexture.Apply();

        var style = new GUIStyle()
        {
            border = new RectOffset(0, 0, 0, 0),
            normal = { background = backgroundTexture }
        };

        GUI.Box(new Rect(0, 0, Screen.width, _scanningArea.y), "", style);
        GUI.Box(new Rect(0, _scanningArea.y + _scanningArea.height, Screen.width, Screen.height - (_scanningArea.y + _scanningArea.height)), "", style);
        GUI.Box(new Rect(0, _scanningArea.y, _scanningArea.x, _scanningArea.height), "", style);
        GUI.Box(new Rect(_scanningArea.x + _scanningArea.width, _scanningArea.y, Screen.width - (_scanningArea.x + _scanningArea.width), _scanningArea.height), "", style);
    }

    private void InitScanningArea()
    {
        _scanningArea = new RectInt(x, y, previewWidth, previewHeight);
    }
    private Texture2D GetImageTexture(XRCpuImage image)
    {
        int xOffset = image.width / 4;
        int yOffset = image.height / 4;
        int regionWidth = image.width / 2;
        int regionHeight = image.height / 2;

        var inputRect = new RectInt(xOffset, yOffset, regionWidth, regionHeight);
        var outputDimensions = new Vector2Int(regionWidth / 2, regionHeight / 2);
        var textureFormat = TextureFormat.ARGB32;

        var conversionParams = new XRCpuImage.ConversionParams
        {
            inputRect = inputRect,
            outputDimensions = outputDimensions,
            outputFormat = textureFormat,
            transformation = XRCpuImage.Transformation.None
        };

        var cameraImageTexture = new Texture2D(regionWidth / 2, regionHeight / 2, textureFormat, false);
        var rawImageData = new NativeArray<byte>(image.GetConvertedDataSize(conversionParams), Allocator.Temp);
        image.Convert(conversionParams, rawImageData);

        cameraImageTexture.LoadRawTextureData(rawImageData);
        cameraImageTexture.Apply();

        rawImageData.Dispose();
        return cameraImageTexture;
    }

}
