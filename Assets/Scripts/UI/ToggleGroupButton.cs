using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToggleGroupButton : Toggle
{
    public event Action<CategoryGroup, bool> OnToggledGroup;
    public TextMeshProUGUI Label;
    public CategoryGroup categoryGroup;
    public Transform Content;

    private int state = 0;

    protected override void Start()
    {
        base.Start();
        this.onValueChanged.AddListener(OnToggledGroupChanged);
    }

    public void PartialFill()
    {
        state = 1;
    }

    private void OnToggledGroupChanged(bool isToggled)
    {
        OnToggledGroup?.Invoke(categoryGroup, isToggled);
    }
}
