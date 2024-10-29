using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ModelManager : MonoBehaviour
{
    public static Action<HashSet<Category>> OnModelsLoaded;
    public List<Transform> models;

    public Dictionary<GameObject, List<string>> keyValuePairs;

    private List<ModelData> modelData;

    [SerializeField]
    private Category[] categories;

    private void Start()
    {
        HashSet<Category> combinedCategories = new HashSet<Category>();
        categories = Resources.LoadAll("", typeof(Category)).Cast<Category>().ToArray();
        modelData = new List<ModelData>();
        foreach (var model in models)
        {
            var data = new ModelData(model.gameObject, categories: categories);
            modelData.Add(data);
            combinedCategories.UnionWith(data.UsedCategories);
            model.gameObject.SetActive(false);
        }
        OnModelsLoaded?.Invoke(combinedCategories);
    }
    private void OnEnable()
    {
        ToggleButtonManager.CategoryToggled += OnCategoryToggled;
        QRScanner.OnQRScanned += EnableModel;
    }

    private void OnDisable()
    {
        ToggleButtonManager.CategoryToggled -= OnCategoryToggled;
        QRScanner.OnQRScanned -= EnableModel;
    }

    private void EnableModel(SnappingPoint sp)
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

}