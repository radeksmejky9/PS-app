using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UI;

public class Collapse : MonoBehaviour
{
    [SerializeField] private GameObject CategoryContent;
    [SerializeField] private GameObject CollapseButton;
    [SerializeField] private RectTransform ArrowTransform;
    private float duration = 0.25f;
    private List<GameObject> children;
    private bool isCollapsed = true;
    private float originalHeight;

    private void Start()
    {
        children = new List<GameObject>();
        GameObjectUtils.GetChildGameObjects(CategoryContent, children);
        duration = duration / children.Count;
        if (children.Count > 0)
        {
            RectTransform childRectTransform = children[0].GetComponent<RectTransform>();
            if (childRectTransform != null)
            {
                originalHeight = childRectTransform.sizeDelta.y;
            }
        }
    }

    public void ToggleCollapse()
    {
        StartCoroutine(AnimateChildren(isCollapsed));
        StartCoroutine(RotateArrow(isCollapsed));
        isCollapsed = !isCollapsed;
    }

    private IEnumerator AnimateChildren(bool collapsing)
    {
        foreach (var child in children)
        {
            ToggleChildObjects(child, !collapsing);
            yield return StartCoroutine(AnimateChildCollapse(child, collapsing));
        }
    }

    private IEnumerator AnimateChildCollapse(GameObject child, bool collapsing)
    {
        RectTransform childRect = child.GetComponent<RectTransform>();
        if (childRect == null) yield break;

        float elapsedTime = 0f;
        float startHeight = collapsing ? originalHeight : 0;
        float endHeight = collapsing ? 0 : originalHeight;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            float newHeight = Mathf.Lerp(startHeight, endHeight, t);
            childRect.sizeDelta = new Vector2(childRect.sizeDelta.x, newHeight);
            yield return null;
        }

        childRect.sizeDelta = new Vector2(childRect.sizeDelta.x, endHeight);
    }

    private IEnumerator RotateArrow(bool collapsing)
    {
        float startRotation = collapsing ? -90 : 0;
        float endRotation = collapsing ? 0 : -90;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            float newRotation = Mathf.Lerp(startRotation, endRotation, t);
            ArrowTransform.rotation = Quaternion.Euler(0, 0, newRotation); // Rotate around the Z-axis
            yield return null;
        }

        ArrowTransform.rotation = Quaternion.Euler(0, 0, endRotation);
    }

    private void ToggleChildObjects(GameObject parent, bool state)
    {
        List<GameObject> categoryChildren = new List<GameObject>();
        GameObjectUtils.GetChildGameObjects(parent, categoryChildren);

        foreach (var categoryChild in categoryChildren)
        {
            categoryChild.SetActive(state);
        }
    }
}
