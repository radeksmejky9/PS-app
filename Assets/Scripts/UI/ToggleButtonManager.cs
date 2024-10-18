using System;
using System.Linq;
using UnityEngine;

public class ToggleButtonManager : MonoBehaviour
{
    public Action<Category, bool> CategoryToggled;
    public Transform parent;
    public ToggleButton tgButtonPrefab;

    private Category[] categories;

    private void Start()
    {
        categories = Resources.LoadAll("", typeof(Category)).Cast<Category>().ToArray();
        CreateCategoryMenu();
    }

    void CreateCategoryMenu()
    {
        foreach (var category in categories)
        {
            ToggleButton toggleObj = Instantiate(tgButtonPrefab, parent);
            toggleObj.Label.text = category.ToString();
            toggleObj.category = category;
        }

    }
}
