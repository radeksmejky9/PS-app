using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ModelManager : MonoBehaviour
{
    public Transform model;

    private ModelData modelData;

    [SerializeField]
    private Category[] categories;

    private void Start()
    {
        categories = Resources.LoadAll("", typeof(Category)).Cast<Category>().ToArray();
        modelData = new ModelData(model.gameObject, categories: categories);
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
        if (isActive)
        {

            modelData.Filter.Add(category);
        }
        else
        {
            modelData.Filter.Remove(category);
        }
        UpdateVisibility();
    }
    private void UpdateVisibility()
    {
        foreach (var pipe in modelData.Pipes)
        {
            pipe.gameObject.SetActive(modelData.Filter.Contains(pipe.Category));
        }
        foreach (var fitting in modelData.Fittings)
        {
            fitting.gameObject.SetActive(modelData.Filter.Contains(fitting.Category));
        }
    }

}