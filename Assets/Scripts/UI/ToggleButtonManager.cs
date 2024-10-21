using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ToggleButtonManager : MonoBehaviour
{
    public static Action<Category, bool> CategoryToggled;
    public Transform parent;

    public ToggleButton tgButtonPrefab;
    public ToggleGroupButton tgGroupButtonPrefab;

    private Category[] categories;
    private CategoryGroup[] categoryGroups;

    private List<ToggleGroupButton> toggleCategoryButtons = new List<ToggleGroupButton>();
    private List<ToggleButton> toggleButtons = new List<ToggleButton>();

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
            groupToggleObj.categoryGroup = group;
            toggleCategoryButtons.Add(groupToggleObj);

            foreach (var category in categories)
            {
                if (category.categoryGroup != group) continue;

                ToggleButton toggleObj = Instantiate(tgButtonPrefab, groupToggleObj.Content.transform);
                toggleObj.Label.text = category.ToString();
                toggleObj.category = category;
                toggleObj.categoryGroup = category.categoryGroup;
                toggleObj.OnToggleChanged += OnCategoryToggle;
                toggleButtons.Add(toggleObj);
                groupToggleObj.toggleButtons.Add(toggleObj);
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
            toggleButtons.Add(toggleObj);
        }
    }

    private void OnCategoryToggle(Category category, CategoryGroup categoryGroup, bool isToggled)
    {
        CategoryToggled?.Invoke(category, isToggled);
        toggleCategoryButtons.ForEach(button =>
        {
            if (button.categoryGroup == categoryGroup)
            {
                button.ChangeState();
            }
        });
    }
    private void OnCategoryGroupToggled(ToggleGroupButton groupButton, bool isToggled)
    {
        groupButton.toggleButtons.ForEach(button => button.isOn = isToggled);
        groupButton.ChangeState();
    }
}
