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

        //CheckIfWeAreOnBreathableScene();




        OverlayCamera = new GameObject().AddComponent<Camera>();
        OverlayCamera.transform.parent = Camera.main.transform;        
        Destroy(OverlayCamera.GetComponentInChildren<AudioListener>());

        //OverlayCamera = Instantiate(Camera.main, Camera.main.transform);
        //Transform[] children = OverlayCamera.GetComponentsInChildren<Transform>(true);

        //for (int i = 0; i < children.Length; i++)
        //{
        //    if (children[i] != OverlayCamera.transform)
        //    {
        //        Destroy(children[i].gameObject);
        //    }
        //}


        OverlayCamera.transform.position = Camera.main.transform.position;
        OverlayCamera.transform.rotation = Camera.main.transform.rotation;
        OverlayCamera.fieldOfView = Camera.main.fieldOfView;

        UniversalAdditionalCameraData data = Camera.main.GetUniversalAdditionalCameraData();
        data.renderType = CameraRenderType.Base;
        data.cameraStack.Add(OverlayCamera);

        UniversalAdditionalCameraData data2 = OverlayCamera.GetUniversalAdditionalCameraData();
        data2.renderType = CameraRenderType.Overlay;
        OverlayLayerMask = (1 << 12);
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


    //public void CheckIfWeAreOnBreathableScene()
    //{
    //    if (GameManager.Instance.CurrentSceneType == GameManager.TypeOfScene.AsteroidField)
    //    {
    //        GameManager.Instance.LifeSupportSystem.OnEnterUnbreathablePlace();
    //    }
    //}
}
