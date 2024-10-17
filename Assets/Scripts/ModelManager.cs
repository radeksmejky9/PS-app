using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ModelManager : MonoBehaviour
{
    private Transform model;

    [SerializeField]
    private Material Undefined;

    [SerializeField]
    private Category[] categories;

    private void OnEnable()
    {
        model = GetComponent<Transform>();
        categories = Resources.LoadAll("", typeof(Category)).Cast<Category>().ToArray();
        Load();
    }

    private void Load()
    {
        List<MeshRenderer> children = model.GetComponentsInChildren<MeshRenderer>().ToList();

        children = IFCOpener.CleanModel(children);
        children.ForEach(child => child.AddComponent<BoxCollider>());

        var connectionChildren = IFCOpener.GetConnections(children);
        var segmentChildren = IFCOpener.GetSegments(children);

        var materialNames = categories.Select(category => category.material.name).ToList();
        var dict = IFCOpener.GetPipes(children, materialNames);

        foreach (KeyValuePair<string, List<MeshRenderer>> sortedChildren in dict)
        {
            var material = categories.Where(category => category.material.name == sortedChildren.Key).FirstOrDefault().material;
            sortedChildren.Value.ForEach(child =>
            {
                child.material = material;
            });
        }

        connectionChildren.ForEach(child =>
        {
            var gameojb = child.GetClosestSegment(segmentChildren.ToArray());
            Material mat;
            if (gameojb != null && gameojb.TryGetComponent<MeshRenderer>(out var material))
            {
                mat = material.material;
            }
            else
            {
                mat = Undefined;
            }
            child.GetComponent<MeshRenderer>().material = mat;
        });
    }

}