using System.Collections;
using UnityEngine;
using ZXing;
using UnityEngine.XR.ARFoundation;
using Unity.Collections;
using UnityEngine.XR.ARSubsystems;
using System;
using Newtonsoft.Json;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class QRScanner : MonoBehaviour
{
    public static Action<SnappingPoint> OnQRScanned;

    public ARCameraManager CameraManager;
    public Image Animation;

    [Range(1, 1920)]
    public int previewWidth;
    [Range(1, 1080)]
    public int previewHeight;

    public bool ScanningMode
    {
        get { return scanningMode; }
        private set
        {
            qrCode = string.Empty;

            if (value == true)
            {
                InitScanningArea();
                StartCoroutine(GetQRCode());
            }

            scanPreviewImage.enabled = value;
            this.Animation.gameObject.SetActive(value);
            scanningMode = value;
        }
    }

    private bool scanningMode = false;
    private string qrCode = string.Empty;
    private RectInt scanningArea;
    private Image scanPreviewImage;

    private int x = 0;
    private int y = 0;

    private void Start()
    {
        x = Screen.width / 2 - previewWidth / 2;
        y = Screen.height / 2 - previewHeight / 2;
        scanPreviewImage = this.gameObject.GetComponent<Image>();
    }

    public void SetScanningMode()
    {
        ScanningMode = !ScanningMode;
    }
    private IEnumerator GetQRCode()
    {
        IBarcodeReader barCodeReader = new BarcodeReader();
        while (string.IsNullOrEmpty(qrCode))
        {
            if (CameraManager.TryAcquireLatestCpuImage(out XRCpuImage image))
            {
                var cameraImageTexture = GetImageTexture(image);
                try
                {
                    var Result = barCodeReader.Decode(cameraImageTexture.GetPixels32(), cameraImageTexture.width, cameraImageTexture.height);
                    if (Result != null)
                    {
                        qrCode = Result.Text;
                        if (!string.IsNullOrEmpty(qrCode))
                        {
                            try
                            {
                                OnQRScanned?.Invoke(SnappingPoint.Decode(qrCode.Base64Decode()));
                                Debug.Log("DECODED TEXT FROM QR: " + qrCode.Base64Decode());
                            }
                            catch
                            {
                                OnQRScanned?.Invoke(JsonConvert.DeserializeObject<SnappingPoint>(qrCode));
                                Debug.Log("DECODED TEXT FROM QR: " + qrCode);
                            }

                            Vibration.Vibrate(100);
                            ScanningMode = false;
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("Failed to scan");
                    Debug.LogError(ex.Message);
                    qrCode = string.Empty;
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
        //if (!ScanningMode)
        return;

        Texture2D backgroundTexture = new Texture2D(1, 1);
        backgroundTexture.SetPixel(0, 0, new Color(0, 0, 0, 0.5f));
        backgroundTexture.Apply();

        var style = new GUIStyle()
        {
            border = new RectOffset(0, 0, 0, 0),
            normal = { background = backgroundTexture }
        };

        GUI.Box(new Rect(0, 0, Screen.width, scanningArea.y), "", style);
        GUI.Box(new Rect(0, scanningArea.y + scanningArea.height, Screen.width, Screen.height - (scanningArea.y + scanningArea.height)), "", style);
        GUI.Box(new Rect(0, scanningArea.y, scanningArea.x, scanningArea.height), "", style);
        GUI.Box(new Rect(scanningArea.x + scanningArea.width, scanningArea.y, Screen.width - (scanningArea.x + scanningArea.width), scanningArea.height), "", style);
    }

    private void InitScanningArea()
    {
        scanningArea = new RectInt(x, y, previewWidth, previewHeight);
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
