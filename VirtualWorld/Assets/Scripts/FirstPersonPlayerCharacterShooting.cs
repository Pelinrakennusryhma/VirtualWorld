using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FirstPersonPlayerCharacterShooting : MonoBehaviour
{
    public static FirstPersonPlayerCharacterShooting Instance;

    public CapsuleCollider StandingCapsule;

    public Camera MainCamera;

    public Camera OverlayCamera;

    public LayerMask OverlayLayerMask;


    private void Awake()
    {
        //Debug.Log("We awoke");
    }

    private void OnDestroy()
    {
        //Debug.Log("We we're destroyed");
    }

    private void Start()
    {
        //Debug.Log("Start called");

        Instance = this;

        OverlayCamera = new GameObject().AddComponent<Camera>();
        OverlayCamera.transform.parent = MainCamera.transform;        
        Destroy(OverlayCamera.GetComponentInChildren<AudioListener>());

        OverlayCamera.transform.position = MainCamera.transform.position;
        OverlayCamera.transform.rotation = MainCamera.transform.rotation;
        OverlayCamera.fieldOfView = MainCamera.fieldOfView;

        UniversalAdditionalCameraData data = MainCamera.GetUniversalAdditionalCameraData();
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
        OverlayCamera.transform.position = MainCamera.transform.position;
        OverlayCamera.transform.rotation = MainCamera.transform.rotation;
        OverlayCamera.fieldOfView = MainCamera.fieldOfView;
    }
}
