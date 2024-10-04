using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class DebugText : MonoBehaviour
{
    [SerializeField]
    private TMP_Text debugText;
    [SerializeField]
    private Transform assets;
    [SerializeField]
    private Transform cam;

    void Update()
    {
        UpdateDebugText();
    }

    public void ActivateDebugText()
    {
        debugText.gameObject.SetActive(!debugText.IsActive());
    }
    private void UpdateDebugText()
    {
        if (!debugText.IsActive()) return;

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
