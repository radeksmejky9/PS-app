using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DebugText : MonoBehaviour
{
    [SerializeField]
    private GameObject debug;

    [SerializeField]
    private Transform assets;
    [SerializeField]
    private TextMeshProUGUI debugText;

    private Transform cam;

    private void Start()
    {
        cam = Camera.main.transform;
    }

    void Update()
    {
        UpdateDebugText();
    }

    public void ActivateDebug()
    {
        debug.SetActive(!debug.activeSelf);
    }
    private void UpdateDebugText()
    {
        if (!debug.activeSelf) return;

        debugText.text = @$"<b><color=#FFD700>Assets Position:</color></b>
X: {assets.position.x}
Y: {assets.position.y}
Z: {assets.position.z}
<b><color=#FFD700>Assets Rotation:</color></b>
X: {assets.rotation.x}
Y: {assets.rotation.y}
Z: {assets.rotation.z}
<b><color=#00FF00>Cam Position:</color></b>
X: {cam.position.x}
Y: {cam.position.y}
Z: {cam.position.z}
<b><color=#00FF00>Cam Rotation:</color></b>
X: {cam.rotation.x}
Y: {cam.rotation.y}
Z: {cam.rotation.z}".Trim();
    }
}
