using System;
using System.Linq;
using UnityEngine;

public class ToggleButtonManager : MonoBehaviour
{
    public static Action<Category, bool> CategoryToggled;
    public Transform parent;
    public ToggleButton tgButtonPrefab;

    private Category[] categories;

    private void Start()
    {
        categories = Resources.LoadAll("", typeof(Category)).Cast<Category>().ToArray();
        CreateCategoryMenu();
    }

    private void CreateCategoryMenu()
    {
        foreach (var category in categories)
        {
            ToggleButton toggleObj = Instantiate(tgButtonPrefab, parent);
            toggleObj.Label.text = category.ToString();
            toggleObj.category = category;
            toggleObj.OnToggleChanged += OnCategoryToggle;
        }

    }
    private void OnCategoryToggle(Category category, bool isToggled)
    {
        CategoryToggled?.Invoke(category, isToggled);
    }
}
