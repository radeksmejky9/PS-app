using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class QRSpawner : MonoBehaviour
{
    public Action<Texture2D> OnQRGenerated;

    public Camera scanningCamera;
    public RenderTexture RenderTexture;
    public TextMeshProUGUI Title;
    public RawImage Image;

    private static QRSpawner _instance;
    public static QRSpawner Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindExistingInstance() ?? CreateNewInstance();
            }
            return _instance;
        }
    }

    public void GenerateTexture(Texture2D qrTexture, string title, System.Action<Texture2D> onComplete)
    {
        StartCoroutine(GenerateTextureCoroutine(qrTexture, title, onComplete));
    }
    private IEnumerator GenerateTextureCoroutine(Texture2D qrTexture, string title, System.Action<Texture2D> onComplete)
    {
        Image.texture = qrTexture;
        Title.text = title;

        yield return new WaitForEndOfFrame();

        Texture2D tex = new Texture2D(RenderTexture.width, RenderTexture.height, TextureFormat.RGB24, false);
        RenderTexture.active = RenderTexture;
        tex.ReadPixels(new Rect(0, 0, RenderTexture.width, RenderTexture.height), 0, 0);
        tex.Apply();
        RenderTexture.active = null;

        onComplete?.Invoke(tex);
    }

    private static QRSpawner FindExistingInstance()
    {
        QRSpawner[] existingInstances = FindObjectsOfType<QRSpawner>();

        if (existingInstances == null || existingInstances.Length == 0) return null;

        return existingInstances[0];
    }

    private static QRSpawner CreateNewInstance()
    {
        var containerGO = new GameObject("__" + typeof(QRSpawner).Name + " (Singleton)");
        return containerGO.AddComponent<QRSpawner>();
    }
}
