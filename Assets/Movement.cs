using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.XR.CoreUtils;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField]
    Transform cam;
    [SerializeField]
    Transform assets;
    [SerializeField]
    List<Transform> snappingPoints;

    public void Move()
    {
        assets.SetPositionAndRotation(new Vector3(
            snappingPoints[0].position.x * -1 + cam.position.x,
            snappingPoints[0].position.y * -1 + cam.position.y,
            snappingPoints[0].position.z * -1 + cam.position.z), cam.rotation);
    }
}
