using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ToggleButtonManager : MonoBehaviour
{
    public static Action<Category, bool> CategoryToggled;
    public Transform parent;
    public ToggleButton tgButtonPrefab;
    public ToggleAllButton tgAllButton;

    public GameObject toggleGroupButtonPrefab;
    private Category[] categories;
    private CategoryGroup[] categoryGroups;

    private List<ToggleGroupButton> toggleCategoryButtons = new List<ToggleGroupButton>();
    private List<ToggleButton> toggleButtons = new List<ToggleButton>();

    private void Awake()
    {
        categoryGroups = Resources.LoadAll("CategoryGroup/", typeof(CategoryGroup)).Cast<CategoryGroup>().ToArray();
        categories = Resources.LoadAll("", typeof(Category)).Cast<Category>().ToArray();
    }

    private void OnEnable()
    {
        tgAllButton.OnToggleAll += OnToggleAll;
        ModelManager.OnModelsLoaded += CreateCategoryMenu;
    }

    private void OnDisable()
    {
        tgAllButton.OnToggleAll -= OnToggleAll;
        ModelManager.OnModelsLoaded -= CreateCategoryMenu;
    }

    private void CreateCategoryMenu(HashSet<Category> usedCategories)
    {
        foreach (CategoryGroup group in categoryGroups)
        {
            var groupCategories = usedCategories.Where(category => category.CategoryGroup == group);
            if (!groupCategories.Any()) continue;

            GameObject toggleGroupButton = Instantiate(toggleGroupButtonPrefab, parent);
            ToggleGroupButton groupToggleObj = toggleGroupButton.GetComponentInChildren<ToggleGroupButton>();
            groupToggleObj.Label.text = group.ToString();
            groupToggleObj.OnToggledGroup += OnCategoryGroupToggled;
            groupToggleObj.categoryGroup = group;
            toggleCategoryButtons.Add(groupToggleObj);

            foreach (var category in groupCategories)
            {
                ToggleButton toggleObj = Instantiate(tgButtonPrefab, groupToggleObj.Content.transform);
                toggleObj.Label.text = category.ToString();
                toggleObj.category = category;
                toggleObj.categoryGroup = category.CategoryGroup;
                toggleObj.OnToggleChanged += OnCategoryToggle;
                toggleButtons.Add(toggleObj);
                groupToggleObj.toggleButtons.Add(toggleObj);
            }
        }

        foreach (var category in usedCategories.Where(c => c.CategoryGroup == null))
        {
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
        tgAllButton.ChangeState(toggleButtons);
    }
    private void OnCategoryGroupToggled(ToggleGroupButton groupButton, bool isToggled)
    {
        groupButton.toggleButtons.ForEach(button => button.isOn = isToggled);
        groupButton.ChangeState();
        tgAllButton.ChangeState(toggleButtons);
    }
    private void OnToggleAll(bool isToggled)
    {
        toggleButtons.ForEach(button => button.isOn = isToggled);
    }
}
