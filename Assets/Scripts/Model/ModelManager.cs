using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ModelManager : MonoBehaviour
{
    public static Action<HashSet<Category>> OnModelsLoaded;
    public List<Transform> models;

    public Dictionary<GameObject, List<string>> keyValuePairs;

    private List<ModelData> modelData = new List<ModelData>();

    private void Start()
    {
        LoadModels();
    }
    private void OnEnable()
    {
        ToggleButtonManager.CategoryToggled += OnCategoryToggled;
        QRScanner.OnQRScanned += EnableModels;
    }

    private void OnDisable()
    {
        ToggleButtonManager.CategoryToggled -= OnCategoryToggled;
        QRScanner.OnQRScanned -= EnableModels;
    }

    private void EnableModels(SnappingPoint sp)
    {
        models.ForEach(model => { model.gameObject.SetActive(true); });
    }

    private void OnCategoryToggled(Category category, bool isActive)
    {
        foreach (var data in modelData)
        {
            if (isActive)
            {

                data.Filter.Add(category);
            }
            else
            {
                data.Filter.Remove(category);
            }
            UpdateVisibility(data);
        }
    }
    private void UpdateVisibility(ModelData data)
    {
        foreach (var element in data.GetElementsOfType<ModelElement>())
        {
            element.gameObject.SetActive(data.Filter.Contains(element.Category));
        }
    }

    private void LoadModels()
    {
        HashSet<Category> combinedCategories = new HashSet<Category>();

        foreach (var model in models)
        {
            var data = new ModelData(model.gameObject, categories: CategoryLoader.Instance.Categories);
            modelData.Add(data);
            combinedCategories.UnionWith(data.UsedCategories);
            model.gameObject.SetActive(false);
        }
        OnModelsLoaded?.Invoke(combinedCategories);
    }
}