using System.Collections;
using UnityEngine;
using ZXing;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using Unity.Collections;
using UnityEngine.XR.ARSubsystems;
using TMPro;
using System;

public class BarcodeScanner : MonoBehaviour
{
    public static Action<SnappingPoint> QRChanged;
    public Image QRScannerPreview;
    public ARCameraManager CameraManager;

    private string QrCode = string.Empty;

    void Start()
    {
        StartCoroutine(GetQRCode());
    }

    private IEnumerator GetQRCode()
    {
        IBarcodeReader barCodeReader = new BarcodeReader();

        while (string.IsNullOrEmpty(QrCode))
        {
            if (CameraManager.TryAcquireLatestCpuImage(out XRCpuImage image))
            {
                var conversionParams = new XRCpuImage.ConversionParams
                {
                    inputRect = new RectInt(0, 0, image.width, image.height),
                    outputDimensions = new Vector2Int(image.width, image.height),
                    outputFormat = TextureFormat.RGBA32,
                    transformation = XRCpuImage.Transformation.None
                };

                var cameraImageTexture = new Texture2D(image.width, image.height, TextureFormat.RGBA32, false);
                var rawImageData = new NativeArray<byte>(image.GetConvertedDataSize(conversionParams), Allocator.Temp);
                image.Convert(conversionParams, rawImageData);

                cameraImageTexture.LoadRawTextureData(rawImageData);
                cameraImageTexture.Apply();

                rawImageData.Dispose();
                image.Dispose();

                try
                {
                    var Result = barCodeReader.Decode(cameraImageTexture.GetPixels32(), cameraImageTexture.width, cameraImageTexture.height);
                    if (Result != null)
                    {
                        QrCode = Result.Text;
                        if (!string.IsNullOrEmpty(QrCode))
                        {
                            QRChanged?.Invoke(JsonUtility.FromJson<SnappingPoint>(QrCode));
                            Debug.Log("DECODED TEXT FROM QR: " + QrCode);
                            QRScannerPreview.gameObject.SetActive(false);
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogWarning(ex.Message);
                }
            }

            yield return null;
        }
    }
}
