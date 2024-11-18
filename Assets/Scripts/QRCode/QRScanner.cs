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
        get => scanningMode;
        private set
        {
            if (scanningMode == value) return;

            scanningMode = value;

            if (scanningMode)
            {
                InitScanningArea();
                scanningCoroutine = StartCoroutine(GetQRCode());
            }
            else
            {
                if (scanningCoroutine != null)
                {
                    StopCoroutine(scanningCoroutine);
                    scanningCoroutine = null;
                }
            }

            scanPreviewImage.enabled = scanningMode;
            //Animation.gameObject.SetActive(scanningMode);
        }
    }

    private bool scanningMode = false;
    private string qrCode = string.Empty;
    private RectInt scanningArea;
    private Image scanPreviewImage;
    private Coroutine scanningCoroutine;

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
            if (!CameraManager.TryAcquireLatestCpuImage(out XRCpuImage image))
            {
                yield return null;
                continue;
            }

            var cameraImageTexture = GetImageTexture(image);
            try
            {
                var result = barCodeReader.Decode(cameraImageTexture.GetPixels32(), cameraImageTexture.width, cameraImageTexture.height);
                if (result != null)
                {
                    qrCode = result.Text;
                    if (!string.IsNullOrEmpty(qrCode))
                    {
                        var point = SnappingPoint.Decode(qrCode.Base64Decode());
                        ContentLoader.Instance.LoadModel(point.Url);
                        //OnQRScanned?.Invoke(point);

                        //Debug.Log("DECODED TEXT FROM QR: " + qrCode.Base64Decode());

                        //Vibration.Vibrate(100);
                        ScanningMode = false;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to scan: {ex.Message}");
                Debug.LogError($"Stacktrace: {ex.StackTrace}");
                qrCode = string.Empty;
            }
            finally
            {
                qrCode = string.Empty;
                image.Dispose();
            }

            yield return null;
        }
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
