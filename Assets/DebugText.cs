using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class DebugText : MonoBehaviour
{
    [SerializeField]
    TMP_Text txt;
    [SerializeField]
    Transform assets;
    [SerializeField]
    Transform cam;


    void Update()
    {
        txt.text = @$"
        XR: {assets.position.x}, {assets.position.y}, {assets.position.z}
        cam: {cam.position.x}, {cam.position.y}, {cam.position.z}
        ";
    }
}
