using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.Mathematics;
using Unity.XR.CoreUtils;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Xbim.Ifc2x3.SharedBldgServiceElements;

public class Movement : MonoBehaviour
{
    [SerializeField]
    private Transform Assets;

    private Transform cam;
    private GameObject currentCube;
    private Transform Model;

    private Material[] materials;

    void Start()
    {
        Model = Assets.parent;
        cam = Camera.main.transform;
        materials = LoadAllMaterials("Assets/Materials/IFC");
        Load();
    }

    public static Material[] LoadAllMaterials(string rootFolder)
    {
        string[] filePaths = Directory.GetFiles(rootFolder, "*.mat", SearchOption.AllDirectories);
        Material[] materials = new Material[filePaths.Length];
        for (int i = 0; i < filePaths.Length; i++)
        {
            string assetPath = filePaths[i].Replace(Application.dataPath, "Assets");
            materials[i] = AssetDatabase.LoadAssetAtPath<Material>(assetPath);
        }

        return materials;
    }

    private void Load()
    {
        List<MeshRenderer> children = Assets.GetChild(0).GetChild(0).GetComponentsInChildren<MeshRenderer>().ToList();
        children = IFCOpener.CleanModel(children);

        var materialNames = materials.Select(material => material.name).ToList();
        var dict = IFCOpener.OpenIFC(children, materialNames);
        foreach (var material in materials) Debug.Log(material);

        foreach (KeyValuePair<string, List<MeshRenderer>> sortedChildren in dict)
        {
            var material = materials.Where(material => material.name == sortedChildren.Key).FirstOrDefault();
            sortedChildren.Value.ForEach(child =>
            {
                child.material = material;
            });
        }
    }

    void OnEnable()
    {
        BarcodeScanner.QRScanned += Move;
    }

    void OnDisable()
    {
        BarcodeScanner.QRScanned -= Move;
    }
    public void Move(SnappingPoint snappingPoint)
    {
        if (currentCube != null && Assets.parent != null)
        {
            Assets.parent = Model;
            Destroy(currentCube);
        }

        Assets.SetPositionAndRotation(new Vector3(
                -snappingPoint.Position.X + cam.position.x,
                -snappingPoint.Position.Y + cam.position.y,
                -snappingPoint.Position.Z + cam.position.z
                ), new quaternion(0, 0, 0, 0));

        currentCube = CreateAnchor();

        Quaternion snappingPointRotation = Quaternion.Euler(0, snappingPoint.Rotation.Y, 0);

        float camYRotation = Camera.main.transform.rotation.eulerAngles.y;
        Quaternion camYRotationOnly = Quaternion.Euler(0, camYRotation, 0);
        currentCube.transform.rotation = snappingPointRotation * camYRotationOnly;
        Assets.gameObject.SetActive(true);
    }

    private GameObject CreateAnchor()
    {
        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.GetComponent<Renderer>().enabled = false;
        cube.transform.position = cam.position;
        cube.transform.parent = Model;
        Assets.parent = cube.transform;
        return cube;
    }
}
