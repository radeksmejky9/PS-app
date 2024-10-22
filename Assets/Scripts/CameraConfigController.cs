using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


public class CameraConfigController : MonoBehaviour
{
    public ARCameraManager arCameraManager;

    private bool _init = false;
    void OnEnable()
    {
        arCameraManager.frameReceived += OnCameraFrameReceived;
    }

    void OnDisable()
    {
        arCameraManager.frameReceived -= OnCameraFrameReceived;
    }

    void OnCameraFrameReceived(ARCameraFrameEventArgs eventArgs)
    {
        if (!_init)
        {
            try
            {
                //In my case 0=640*480, 1= 1280*720, 2=1920*1080
                arCameraManager.subsystem.currentConfiguration = arCameraManager.GetConfigurations(Allocator.Temp)[2];
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);

            }
            finally
            {
                _init = true;
            }
        }
    }
}

