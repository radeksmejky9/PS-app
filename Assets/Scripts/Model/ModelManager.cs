using DocumentFormat.OpenXml.Drawing.Charts;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ModelManager : MonoBehaviour
{
    public List<Transform> models;

    private List<ModelData> modelData;

    [SerializeField]
    private Category[] categories;

    private void Start()
    {
        categories = Resources.LoadAll("", typeof(Category)).Cast<Category>().ToArray();
        modelData = new List<ModelData>();
        foreach (var model in models)
        {
            modelData.Add(new ModelData(model.gameObject, categories: categories));
        }
    }
    private void OnEnable()
    {
        ToggleButtonManager.CategoryToggled += OnCategoryToggled;
    }

    private void OnDisable()
    {
        ToggleButtonManager.CategoryToggled -= OnCategoryToggled;
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
        foreach (var element in data.Elements)
        {
            element.gameObject.SetActive(data.Filter.Contains(element.Category));
        }
    }

}