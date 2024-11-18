using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.AsyncOperations;
using Dummiesman;

public class ContentLoader : MonoSingleton<ContentLoader>
{
    public static Action<List<Transform>> OnModelLoad;
    public static Action OnDownloadStart;
    public static Action OnDownloadEnd;
    public static Action<float> OnDownloadProgressChanged;

    private readonly string SERVER_URL = "http://www.etikos.cz/data/";

    public List<Transform> Models => loadedModels;
    public List<Category> Categories
    {
        get
        {
            if (loadedCategories == null)
            {
                Debug.LogError("Categories accessed before initialization!");
            }
            Debug.Log($"Accessing Categories: {loadedCategories?.Count ?? 0} items");
            return loadedCategories;
        }
    }
    public List<CategoryGroup> CategoryGroups => loadedCategoryGroups;

    private List<Category> loadedCategories = new List<Category>();
    private List<CategoryGroup> loadedCategoryGroups = new List<CategoryGroup>();
    private List<Transform> loadedModels = new List<Transform>();
    private OBJLoader Loader => loader ??= new OBJLoader { SplitMode = SplitMode.Object };
    private OBJLoader loader;
    protected override void Awake()
    {
        base.Awake();
        StartCoroutine(LoadDataCoroutine());
    }

    private IEnumerator LoadDataCoroutine()
    {
        yield return LoadDataAsync("Category Group", loadedCategoryGroups);
        yield return LoadDataAsync("Category", loadedCategories);
#if UNITY_EDITOR
        LoadModel("main.obj", progress => OnDownloadProgressChanged?.Invoke(progress));
#endif
    }

    public IEnumerator LoadDataAsync<T>(string label, List<T> list)
    {
        AsyncOperationHandle<IList<T>> handle = Addressables.LoadAssetsAsync<T>(label, null);

        yield return handle;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            list.AddRange(handle.Result);
            Debug.Log($"Loaded {list.Count} items labeled '{label}'");
        }
        else
        {
            Debug.LogError($"Failed to load items with the label '{label}'");
        }

        //Addressables.Release(handle);
    }

    public void LoadModel(string modelName, Action<float> onProgress = null)
    {
        string tempPath = Path.Combine(Application.temporaryCachePath, modelName);
        var model = GetModelIfExists(tempPath);
        if (model != null)
        {
            Models.Add(model.transform);
            OnModelLoad?.Invoke(Models);
            return;
        }


        StartCoroutine(DownloadModel(modelName, tempPath));
    }

    private GameObject GetModelIfExists(string path)
    {
        if (!File.Exists(path))
        {
            Debug.LogWarning("File not found in temporary cache. Attempting to download.");
            return null;
        }

        OnDownloadStart?.Invoke();
        Debug.Log("Model already exists, loading from temporary cache.");
        GameObject model = Loader.Load(path);

        if (model == null)
        {
            Debug.LogError("Model import failed from temporary cache.");
            return null;
        }

        Debug.Log("Model loaded from temporary cache.");
        OnDownloadEnd?.Invoke();
        return model;
    }

    private IEnumerator DownloadModel(string modelName, string path)
    {
        var serverPath = $"{SERVER_URL}/{modelName}";
        UnityWebRequest request = UnityWebRequest.Get(serverPath);

        request.SendWebRequest();
        OnDownloadStart?.Invoke();

        while (!request.isDone)
        {
            OnDownloadProgressChanged?.Invoke(request.downloadProgress);
            yield return null;
        }

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"Model download failed: {request.error}");
            yield break;
        }


        OnDownloadProgressChanged?.Invoke(request.downloadProgress);
        byte[] modelData = request.downloadHandler.data;

        File.WriteAllBytes(path, modelData);
        GameObject model = Loader.Load(path);

        if (model != null)
        {
            Debug.Log("Model downloaded and loaded from temporary cache.");
            Models.Add(model.transform);
            OnModelLoad?.Invoke(Models);
        }
        else
        {
            Debug.LogError("Model import failed from temporary cache.");
        }
        OnDownloadEnd?.Invoke();
    }

}

