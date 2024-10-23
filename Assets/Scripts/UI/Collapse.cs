using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UI;

public class Collapse : MonoBehaviour
{
    [SerializeField] private GameObject categoryContent;
    private RectTransform parentRectTransform;
    [SerializeField] private RectTransform categoryContainerRectTransform;
    [SerializeField] private GameObject collapseButton;
    private RectTransform categoryContentRectTransform;
    private RectTransform arrowTransform;
    private List<GameObject> children;
    private bool isCollapsed = true;

    private void Start()
    {
        children = new List<GameObject>();
        GameObjectUtils.GetChildGameObjects(categoryContent, children);
        arrowTransform = collapseButton.GetComponent<RectTransform>();
        categoryContentRectTransform = categoryContent.GetComponent<RectTransform>();
        parentRectTransform = categoryContainerRectTransform.GetComponentInParent<RectTransform>();
    }

    public void ToggleCollapse()
    {
        ToggleChildrenVisibility(isCollapsed);
        RotateArrow(isCollapsed);
        isCollapsed = !isCollapsed;

        LayoutRebuilder.ForceRebuildLayoutImmediate(parentRectTransform);
        LayoutRebuilder.ForceRebuildLayoutImmediate(categoryContainerRectTransform);
        LayoutRebuilder.ForceRebuildLayoutImmediate(categoryContentRectTransform);
    }

    private void ToggleChildrenVisibility(bool collapsing)
    {
        foreach (var child in children)
        {
            child.SetActive(!collapsing);
        }
    }

    private void RotateArrow(bool collapsing)
    {
        float rotationAngle = collapsing ? 0 : -90;
        arrowTransform.rotation = Quaternion.Euler(0, 0, rotationAngle);
    }
}
