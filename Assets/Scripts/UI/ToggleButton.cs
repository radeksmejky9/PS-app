using DocumentFormat.OpenXml.Drawing.Charts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{
    public event Action<Category, bool> OnToggleChanged;

    public TextMeshProUGUI Label;
    public Toggle toggle;
    public float height;
    public Category category;

    private void Start()
    {
        toggle.onValueChanged.AddListener(OnToggleValueChanged);
    }

    private void OnToggleValueChanged(bool isToggled)
    {
        OnToggleChanged?.Invoke(category, isToggled);
    }
}
