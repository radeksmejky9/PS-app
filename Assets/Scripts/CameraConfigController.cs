using Unity.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;


public class CameraConfigController : MonoBehaviour
{
    public ARCameraManager arCameraManager;

    private bool _init = false;
    private void OnEnable()
    {
        arCameraManager.frameReceived += OnCameraFrameReceived;
    }

    private void OnDisable()
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
            catch
            {
                arCameraManager.subsystem.currentConfiguration = arCameraManager.GetConfigurations(Allocator.Temp)[0];
            }
            finally
            {
                _init = true;
                arCameraManager.frameReceived -= OnCameraFrameReceived;
            }
        }
    }
}

