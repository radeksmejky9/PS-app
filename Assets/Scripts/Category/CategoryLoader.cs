using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class CategoryLoader : MonoSingleton<CategoryLoader>
{
    public List<Category> Categories => loadedCategories;
    public List<CategoryGroup> CategoryGroups => loadedCategoryGroups;

    private List<Category> loadedCategories = new List<Category>();
    private List<CategoryGroup> loadedCategoryGroups = new List<CategoryGroup>();
    private void Awake()
    {
        LoadDataSync("Category", loadedCategories);
        LoadDataSync("Category Group", loadedCategoryGroups);
    }
    public void LoadDataSync<T>(string label, List<T> list)
    {
        AsyncOperationHandle<IList<T>> handle = Addressables.LoadAssetsAsync<T>(label, null);

        handle.WaitForCompletion();

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            list.AddRange(handle.Result);
            Debug.Log($"Loaded {list.Count} items labeled '{label}'");
        }
        else
        {
            Debug.LogError($"Failed to load items with the label '{label}'");
        }

        Addressables.Release(handle);
    }
}
