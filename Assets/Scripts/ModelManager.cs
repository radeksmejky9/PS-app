using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class ModelManager : MonoBehaviour
{
    private Transform model;
    [SerializeField]
    private Material[] materials;

    private void OnEnable()
    {
        model = GetComponent<Transform>();
        Load();
    }

    private void Load()
    {
        List<MeshRenderer> children = model.GetComponentsInChildren<MeshRenderer>().ToList();

        children = IFCOpener.CleanModel(children);

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

        /*connectionChildren.ForEach(child =>
        {
            var material = child.GetClosestSegment(segmentChildren.ToArray()).GetComponent<MeshRenderer>().material;
            Debug.Log(material);
            child.GetComponent<MeshRenderer>().material = material;
        });*/
    }
}