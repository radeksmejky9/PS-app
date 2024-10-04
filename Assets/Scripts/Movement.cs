using System.Collections.Generic;
using Unity.Mathematics;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class Movement : MonoBehaviour
{
    [SerializeField]
    private Transform cam;
    [SerializeField]
    private Transform Assets;
    [SerializeField]
    private List<Transform> snappingPoints;
    [SerializeField]
    private ARTrackedImageManager imageManager;

    private GameObject currentCube;
    private Transform Model;

    void Start()
    {
        Model = Assets.parent;
    }

    void OnEnable()
    {
        imageManager.trackedImagesChanged += OnChanged;
    }

    void OnDisable() { imageManager.trackedImagesChanged -= OnChanged; }


    void Update()
    {
        //Move();
    }


    public void Move()
    {
        if (currentCube != null && Assets.parent != null)
        {
            Assets.parent = Model;
            Destroy(currentCube);
        }

        Assets.position = new Vector3(
                -snappingPoints[1].position.x + cam.position.x,
                -snappingPoints[1].position.y + cam.position.y,
                -snappingPoints[1].position.z + cam.position.z
                );
        Assets.rotation = new quaternion(0, 0, 0, 0);
        currentCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        currentCube.GetComponent<Renderer>().enabled = false;
        currentCube.transform.position = cam.position;
        currentCube.transform.parent = Model;
        Assets.parent = currentCube.transform;

        Vector3 snappingPointEuler = snappingPoints[1].eulerAngles;
        float snappingPointY = snappingPointEuler.y;
        Quaternion snappingPointRotation = Quaternion.Euler(0, snappingPointY, 0);
        float camYRotation = cam.rotation.eulerAngles.y;
        Quaternion camYRotationOnly = Quaternion.Euler(0, camYRotation, 0);
        currentCube.transform.rotation = snappingPointRotation * camYRotationOnly;
    }

    private void OnChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var newImage in eventArgs.added)
        {
            Move();
            foreach (var image in imageManager.trackables)
            {
                image.destroyOnRemoval = true;
            }
        }

        foreach (var updatedImage in eventArgs.updated)
        {
            // Handle updated event
        }

        foreach (var removedImage in eventArgs.removed)
        {
            // Handle removed event
        }
    }
}
