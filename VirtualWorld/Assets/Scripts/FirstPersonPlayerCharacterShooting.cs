using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FirstPersonPlayerCharacterShooting : MonoBehaviour
{
    public static FirstPersonPlayerCharacterShooting Instance;

    public CapsuleCollider StandingCapsule;

    public Camera OverlayCamera;

    public LayerMask OverlayLayerMask;


    private void Awake()
    {
        Instance = this;

        OverlayCamera = new GameObject().AddComponent<Camera>();
        OverlayCamera.transform.parent = Camera.main.transform;        
        Destroy(OverlayCamera.GetComponentInChildren<AudioListener>());

        OverlayCamera.transform.position = Camera.main.transform.position;
        OverlayCamera.transform.rotation = Camera.main.transform.rotation;
        OverlayCamera.fieldOfView = Camera.main.fieldOfView;

        UniversalAdditionalCameraData data = Camera.main.GetUniversalAdditionalCameraData();
        data.renderType = CameraRenderType.Base;
        data.cameraStack.Add(OverlayCamera);

        UniversalAdditionalCameraData data2 = OverlayCamera.GetUniversalAdditionalCameraData();
        data2.renderType = CameraRenderType.Overlay;
        OverlayCamera.cullingMask = OverlayLayerMask;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        OverlayCamera.transform.position = Camera.main.transform.position;
        OverlayCamera.transform.rotation = Camera.main.transform.rotation;
        OverlayCamera.fieldOfView = Camera.main.fieldOfView;
    }
}
