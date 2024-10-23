using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : Toggle
{
    public event Action<Category, CategoryGroup, bool> OnToggleChanged;
    public TextMeshProUGUI Label;
    public Category category;
    public CategoryGroup categoryGroup;

    public Image img;

    protected override void Start()
    {
        base.Start();
        img.material = category.material;

        this.onValueChanged.AddListener(OnToggleValueChanged);
    }

    private void OnToggleValueChanged(bool isToggled)
    {
        OnToggleChanged?.Invoke(category, categoryGroup, isToggled);
    }
}
