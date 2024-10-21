using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ToggleButtonManager : MonoBehaviour
{
    public static Action<Category, bool> CategoryToggled;
    public static Action<CategoryGroup, bool> CategoryGroupToggled;
    public Transform parent;

    public ToggleButton tgButtonPrefab;
    public ToggleGroupButton tgGroupButtonPrefab;

    private Category[] categories;
    private CategoryGroup[] categoryGroups;

    private void Start()
    {
        categoryGroups = Resources.LoadAll("CategoryGroup/", typeof(CategoryGroup)).Cast<CategoryGroup>().ToArray();
        categories = Resources.LoadAll("", typeof(Category)).Cast<Category>().ToArray();
        CreateCategoryMenu();
    }

    private void CreateCategoryMenu()
    {
        foreach (CategoryGroup group in categoryGroups)
        {
            ToggleGroupButton groupToggleObj = Instantiate(tgGroupButtonPrefab, parent);
            groupToggleObj.Label.text = group.ToString();
            groupToggleObj.OnToggledGroup += OnCategoryGroupToggled;

            foreach (var category in categories)
            {
                if (category.categoryGroup != group) continue;

                ToggleButton toggleObj = Instantiate(tgButtonPrefab, groupToggleObj.Content.transform);
                toggleObj.Label.text = category.ToString();
                toggleObj.category = category;
                toggleObj.categoryGroup = category.categoryGroup;
                toggleObj.OnToggleChanged += OnCategoryToggle;
                //toggleObj.transform.localPosition = Vector3.zero;
            }
        }

        foreach (var category in categories)
        {
            if (category.categoryGroup != null) continue;

            ToggleButton toggleObj = Instantiate(tgButtonPrefab, parent);
            toggleObj.Label.text = category.ToString();
            toggleObj.category = category;
            toggleObj.categoryGroup = null;
            toggleObj.OnToggleChanged += OnCategoryToggle;
        }
    }

    private void OnCategoryToggle(Category category, bool isToggled)
    {
        CategoryToggled?.Invoke(category, isToggled);
    }
    private void OnCategoryGroupToggled(CategoryGroup group, bool isToggled)
    {
        CategoryGroupToggled?.Invoke(group, isToggled);
    }
}
