using System.Collections.Generic;
using Unity.Mathematics;
using Unity.XR.CoreUtils;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField]
    private Transform cam;
    [SerializeField]
    private Transform Assets;
    [SerializeField]
    private List<Transform> snappingPoints;

    private GameObject currentCube;
    private Transform Model;

    void Start()
    {
        Model = Assets.parent;
    }

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
                -snappingPoints[0].position.x + cam.position.x,
                -snappingPoints[0].position.y + cam.position.y,
                -snappingPoints[0].position.z + cam.position.z);
        Assets.rotation = new quaternion(0, 0, 0, 0);
        currentCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        currentCube.transform.position = cam.position;
        currentCube.transform.parent = Model;
        Assets.parent = currentCube.transform;
        quaternion snappingPointRotation = quaternion.Euler(snappingPoints[0].eulerAngles);
        currentCube.transform.rotation = snappingPointRotation * cam.rotation;
    }
}
