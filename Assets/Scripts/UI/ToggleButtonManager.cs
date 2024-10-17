using System;
using System.Linq;
using UnityEngine;

public class ToggleButtonManager : MonoBehaviour
{
    public Action<Category, bool> CategoryToggled;
    public Transform parent;
    public ToggleButton tgButtonPrefab;
    public RectTransform viewport;

    private Category[] categories;
    private float height = 0;

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
            height += toggleObj.height;

        }
        RectTransform viewportRect = viewport.GetComponent<RectTransform>();
        viewportRect.sizeDelta = new Vector2(viewportRect.sizeDelta.x, height);

    }
}
