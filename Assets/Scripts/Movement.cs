using Unity.Mathematics;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField]
    private Transform Assets;

    private Transform cam;
    private GameObject currentCube;
    private Transform Model;
    void Start()
    {
        Model = Assets.parent;
        cam = Camera.main.transform;
        Assets.gameObject.SetActive(false);
    }
    void OnEnable()
    {
        QRScanner.OnQRScanned += Move;
    }

    void OnDisable()
    {
        QRScanner.OnQRScanned -= Move;
    }
    public void Move(SnappingPoint snappingPoint)
    {
        if (currentCube != null && Assets.parent != null)
        {
            Assets.parent = Model;
            Destroy(currentCube);
        }

        Assets.SetPositionAndRotation(new Vector3(
                -snappingPoint.Position.x + cam.position.x,
                -snappingPoint.Position.y + cam.position.y,
                -snappingPoint.Position.z + cam.position.z
                ), new quaternion(0, 0, 0, 0));

        currentCube = CreateAnchor();

        Quaternion snappingPointRotation = Quaternion.Euler(0, snappingPoint.Rotation, 0);

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
