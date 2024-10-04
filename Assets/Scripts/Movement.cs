using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField]
    private Transform cam;
    [SerializeField]
    private Transform Model;
    [SerializeField]
    private List<Transform> snappingPoints;

    public void Move()
    {
        //Model.rotation = cam.rotation;
        Model.position = new Vector3(
            -snappingPoints[0].position.x + cam.position.x,
            -snappingPoints[0].position.y + cam.position.y,
            -snappingPoints[0].position.z + cam.position.z);
    }
}
