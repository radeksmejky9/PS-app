using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using AsImpL;
using System.IO;

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

    private MultiObjectImporter MultiObjectImporter;
    private List<Category> loadedCategories = new List<Category>();
    private List<CategoryGroup> loadedCategoryGroups = new List<CategoryGroup>();
    private List<Transform> loadedModels = new List<Transform>();
    protected override void Awake()
    {
        base.Awake();
        StartCoroutine(LoadDataCoroutine());
        MultiObjectImporter = this.GetComponent<MultiObjectImporter>();
    }

    private IEnumerator LoadDataCoroutine()
    {
        yield return LoadDataAsync("Category Group", loadedCategoryGroups);
        yield return LoadDataAsync("Category", loadedCategories);
#if UNITY_EDITOR
        MultiObjectImporter.ImportModelAsync("Model", Path.Combine(SERVER_URL, "main.obj"), this.transform, MultiObjectImporter.defaultImportOptions);
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
}

