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
    private Material[] materials;
    [SerializeField]
    private Material Undefined;

    private void OnEnable()
    {
        model = GetComponent<Transform>();
        Load();
    }

    private void Load()
    {
        List<MeshRenderer> children = model.GetComponentsInChildren<MeshRenderer>().ToList();

        children = IFCOpener.CleanModel(children);
        children.ForEach(child => child.AddComponent<BoxCollider>());

        var connectionChildren = IFCOpener.GetConnections(children);
        var segmentChildren = IFCOpener.GetSegments(children);

        var materialNames = materials.Select(material => material.name).ToList();
        var dict = IFCOpener.GetPipes(children, materialNames);

        foreach (KeyValuePair<string, List<MeshRenderer>> sortedChildren in dict)
        {
            var material = materials.Where(material => material.name == sortedChildren.Key).FirstOrDefault();
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