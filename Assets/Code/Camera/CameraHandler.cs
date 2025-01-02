using System;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    public static Action OnLevelLoaded;
    public static Action OnMenuLoaded;

    Camera mainSceneCamera;

    private void Awake()
    {
        mainSceneCamera = Camera.main;
    }
    private void OnEnable()
    {
        OnLevelLoaded += CamSwitchOff;
        OnMenuLoaded += CamSwitchOn;
    }
    private void OnDisable()
    {
        OnLevelLoaded -= CamSwitchOff;
        OnMenuLoaded -= CamSwitchOn;
    }

    void CamSwitchOff()
    {
        if (mainSceneCamera != null) mainSceneCamera.enabled = false;
        else Debug.Log("Camera é NULL");
    }

    void CamSwitchOn()
    {
        if (mainSceneCamera != null) mainSceneCamera.enabled = true;
        else Debug.Log("Camera é NULL");
    }    
}
