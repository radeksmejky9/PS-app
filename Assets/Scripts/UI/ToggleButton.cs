using System;
using TMPro;
using UnityEngine.UI;

public class ToggleButton : Toggle
{
    public event Action<Category, bool> OnToggleChanged;
    public TextMeshProUGUI Label;
    public Category category;
    public CategoryGroup categoryGroup;

    protected override void Start()
    {
        base.Start();
        this.onValueChanged.AddListener(OnToggleValueChanged);
    }

    private void OnToggleValueChanged(bool isToggled)
    {
        OnToggleChanged?.Invoke(category, isToggled);
    }
}
