using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.ARSubsystems;

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
        assets:
        {assets.position.x}
        {assets.position.y}
        {assets.position.z}
        assets rotation:
        {assets.rotation.x}
        {assets.rotation.y}
        {assets.rotation.z}
        cam:
        {cam.position.x}
        {cam.position.y}
        {cam.position.z}
        cam rotation:
        {cam.rotation.x}
        {cam.rotation.y}
        {cam.rotation.z}
        ";
    }
}
