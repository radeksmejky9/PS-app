using System;
using System.Collections.Generic;
using UnityEngine;


public class ModelManager : MonoBehaviour
{
    public static Action<HashSet<Category>> OnModelsLoaded;
    public Transform Assets;
    public Dictionary<GameObject, List<string>> keyValuePairs;

    private List<Transform> models;
    private List<ModelData> modelData = new List<ModelData>();

    private void OnEnable()
    {
        ToggleButtonManager.CategoryToggled += OnCategoryToggled;
        ContentLoader.OnModelLoad += LoadModels;
        QRScanner.OnQRScanned += EnableModels;
    }

    private void OnDisable()
    {
        ToggleButtonManager.CategoryToggled -= OnCategoryToggled;
        ContentLoader.OnModelLoad -= LoadModels;
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

    private void LoadModels(List<Transform> models)
    {
        this.models = models;
        HashSet<Category> combinedCategories = new HashSet<Category>();

        foreach (var model in this.models)
        {
            var data = new ModelData(model.gameObject, categories: ContentLoader.Instance.Categories);
            modelData.Add(data);
            combinedCategories.UnionWith(data.UsedCategories);
            model.parent = Assets;
            model.gameObject.SetActive(true);
        }
        OnModelsLoaded?.Invoke(combinedCategories);
    }
}
