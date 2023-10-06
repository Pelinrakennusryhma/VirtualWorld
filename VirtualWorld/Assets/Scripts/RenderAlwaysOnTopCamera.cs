using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderAlwaysOnTopCamera : MonoBehaviour
{
    private Camera RenderOnTopCamera;

    public void Awake()
    {
        RenderOnTopCamera = GetComponent<Camera>();
        RenderOnTopCamera.enabled = false;
    }

    public void EnableRenderOnTopCamera()
    {
        RenderOnTopCamera.enabled = true;
    }

    public void DisableRenderOnTopCamera()
    {
        if(RenderOnTopCamera == null)
        {
            RenderOnTopCamera = GetComponent<Camera>();
        }
        RenderOnTopCamera.enabled = false;
    }

    public void SetFieldOfView(float fov)
    {
        RenderOnTopCamera.fieldOfView = fov;
    }
}
