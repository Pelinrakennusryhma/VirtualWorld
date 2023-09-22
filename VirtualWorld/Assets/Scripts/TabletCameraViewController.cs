using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using FishNet.Object;

public class TabletCameraViewController : NetworkBehaviour
{
    public Camera ThirdPersonCamera;
    public CinemachineVirtualCamera CloseupCamera;
    public CinemachineVirtualCamera TransitionPos1Camera;
    public CinemachineVirtualCamera TransitionPos2Camera;

    public CinemachineVirtualCamera MainVirtualCamera;

    public CinemachineBrain CinemachineBrain;

    public StarterAssetsInputs Inputs;

    public bool IsActiveTabletView;

    public GameObject Graphics;

    public bool HasReachedTransitionPos;

    public bool IsReachingToTransitionPos1;

    public bool IsTakenOverByCheapInterpolations;

    public GameObject PlayerCameraRoot;

    public Camera MapCamera;

    public Vector3 MapStartPos;
    public Quaternion MapStartRot;
    public float MapStartFarClipPlane;

    public GameObject GreenBlip;
    public GameObject YellowBlip;

    public RenderAlwaysOnTopCamera RenderAlwaysOnTopCamera;

    public bool WaitingToBreak;

    public Camera FlyCamera;

    public ThirdPersonController ThirdPersonController;

    private float incomingScaleSpeed = 10.0f;
    private float outgoingScaleSpeed = 10.0f;

    public GameObject TabletScaler;
    private Vector3 OriginalScalerScale;

    private TabletFunctionalityController TabletFunctionality;

    public bool WentOverLeftShoulder;
    public InventoryViewChanger InventoryViewChanger;


    private void Awake()
    {
        TabletFunctionality = GetComponent<TabletFunctionalityController>();
        ThirdPersonCamera = Camera.main;

        if(ThirdPersonCamera != null)
        {
            CinemachineBrain = ThirdPersonCamera.GetComponent<CinemachineBrain>();
        }

    }

    public override void OnStartClient()
    {
        OriginalScalerScale = TabletScaler.transform.localScale;

        IsActiveTabletView = false;
        Graphics.gameObject.SetActive(false);
        TransitionPos1Camera.gameObject.SetActive(false);
        CloseupCamera.gameObject.SetActive(false);
        MapCamera.gameObject.SetActive(false);


        YellowBlip.gameObject.SetActive(!IsOwner);
        GreenBlip.gameObject.SetActive(IsOwner);


        RenderAlwaysOnTopCamera.DisableRenderOnTopCamera();

        FlyCamera.enabled = false;
    }

    public void OnTabletPressed()
    {
        //Debug.Log("Tablet button was pressed " + Time.time);
        //Inputs.ClearTabletInput();


        IsTakenOverByCheapInterpolations = true;

        if (!IsActiveTabletView)
        {
            SetupTabletForComingIn();
            TabletFunctionality.OnTabletOpened();
#if UNITY_WEBGL
            Inputs.UnlockCursor();
#endif
        }

        else
        {
            SetupTabletForGoingOut();
#if UNITY_WEBGL
            Inputs.LockCursor();
#endif
        }
    }

    #region SetupTransitionBeginnings

    private void SetupTabletForComingIn()
    {
        InventoryViewChanger.CameraStartedTransitioning();

        TabletScaler.transform.localScale = Vector3.zero;

        ThirdPersonController.OnTabletViewChanged(true);
        YellowBlip.SetActive(false);
        GreenBlip.SetActive(true);

        MapCamera.gameObject.SetActive(true);

        MapStartPos = ThirdPersonCamera.transform.position;
        MapStartRot = ThirdPersonCamera.transform.rotation;

        HasReachedTransitionPos = false;

        IsActiveTabletView = true;
        //CinemachineBrain.enabled = false;

        Graphics.gameObject.SetActive(true);

        float magnitudeToPos1 = (ThirdPersonCamera.transform.position - TransitionPos1Camera.transform.position).magnitude;
        float magnitudeToPos2 = (ThirdPersonCamera.transform.position - TransitionPos2Camera.transform.position).magnitude;

        ThirdPersonCamera.enabled = false;
        FlyCamera.enabled = true;
        FlyCamera.fieldOfView = ThirdPersonCamera.fieldOfView;
        FlyCamera.transform.position = ThirdPersonCamera.transform.position;
        FlyCamera.transform.rotation = ThirdPersonCamera.transform.rotation;

        RenderAlwaysOnTopCamera.SetFieldOfView(ThirdPersonCamera.fieldOfView);

        if (magnitudeToPos1 <= magnitudeToPos2)
        {
            IsReachingToTransitionPos1 = true;
            WentOverLeftShoulder = false;
        }

        else
        {
            IsReachingToTransitionPos1 = false;
            WentOverLeftShoulder = true;
        }

        InventoryViewChanger.SetWentOverLeftShoulder(WentOverLeftShoulder);
    }

    private void SetupTabletForGoingOut()
    {
        InventoryViewChanger.CameraStartedTransitioning();
        RenderAlwaysOnTopCamera.DisableRenderOnTopCamera();
        HasReachedTransitionPos = false;

        IsActiveTabletView = false;
        //CinemachineBrain.enabled = true;

        //Graphics.gameObject.SetActive(false);

        float magnitudeToPos1 = (ThirdPersonCamera.transform.position - TransitionPos1Camera.transform.position).magnitude;
        float magnitudeToPos2 = (ThirdPersonCamera.transform.position - TransitionPos2Camera.transform.position).magnitude;

        if (magnitudeToPos1 <= magnitudeToPos2)
        {
            IsReachingToTransitionPos1 = true;
        }

        else
        {
            IsReachingToTransitionPos1 = false;
        }
    }

    #endregion

    // Update is called once per frame
    void LateUpdate()
    {
        if (!IsOwner)
        {
            return;
        }

        if (Inputs.tablet)
        {
            Inputs.ClearTabletInput();

            if (ThirdPersonController.Grounded) 
            {
                OnTabletPressed();
            }
        }

        if (!IsTakenOverByCheapInterpolations)
        {
            return;
        }

        SetMapCameraPositionAndRotation();

        if (IsActiveTabletView)
        {
            ScaleInTabletObject();

            if (!HasReachedTransitionPos)
            {
                ReachToTransitionPosition();
            }

            else
            {
                ReachToCloseUpPosition();
            }

        }

        else
        {
            if (!HasReachedTransitionPos)
            {
                ReachOutToTransitionPosition();
            }

            else
            {
                ReachOutToGamePlayView();
            }
        }
    }

    #region ReachingToPositions

    private void ReachOutToGamePlayView()
    {
        //Vector3 targetPos = MainVirtualCamera.transform.position;
        //Quaternion targetRot = MainVirtualCamera.transform.rotation;

        //Quaternion targetRot = Quaternion.LookRotation(PlayerCameraRoot.transform.position - MainVirtualCamera.transform.position);

        Vector3 targetPos = ThirdPersonCamera.transform.position;
        Quaternion targetRot = ThirdPersonCamera.transform.rotation;

        //ThirdPersonCamera.transform.position = Vector3.Lerp(ThirdPersonCamera.transform.position, targetPos, Time.deltaTime * 5.0f);
        //ThirdPersonCamera.transform.rotation = Quaternion.Lerp(ThirdPersonCamera.transform.rotation, targetRot, Time.deltaTime * 5.6f);

        FlyCamera.transform.position = Vector3.Lerp(FlyCamera.transform.position, targetPos, Time.deltaTime * 5.0f);
        FlyCamera.transform.rotation = Quaternion.Lerp(FlyCamera.transform.rotation, targetRot, Time.deltaTime * 5.6f);




        //float magnitudeToTargetPos = (ThirdPersonCamera.transform.position - MainVirtualCamera.transform.position).magnitude;

        //float angleBetweenRots = Quaternion.Angle(ThirdPersonCamera.transform.rotation, targetRot);



        float magnitudeToTargetPos = (FlyCamera.transform.position - ThirdPersonCamera.transform.position).magnitude;

        float angleBetweenRots = Quaternion.Angle(FlyCamera.transform.rotation, targetRot);

        ScaleTabletObjectOut();

        if (magnitudeToTargetPos <= 0.005f
            && angleBetweenRots <= 0.0001f)
        {
            WaitingToBreak = true;
            //Debug.Break();
            //Debug.Log("Third person pos is " + ThirdPersonCamera.transform.position + " virtual camera pos is " + MainVirtualCamera.transform.position);
            //Debug.Log("Third person eueler rot is " + ThirdPersonCamera.transform.rotation.eulerAngles + " virtual camera rot is " + MainVirtualCamera.transform.rotation.eulerAngles);
            StopMessingWithCameras();
            TabletFunctionality.OnTabletClosed();
        }

        if (magnitudeToTargetPos <= 0.01f)
        {
            //Debug.Break();
        }
    }



    private void ReachOutToTransitionPosition()
    {
        Vector3 targetPos;
        Quaternion targetRot;

        if (IsReachingToTransitionPos1)
        {
            targetPos = TransitionPos1Camera.transform.position;
            targetRot = TransitionPos1Camera.transform.rotation;
        }

        else
        {
            targetPos = TransitionPos2Camera.transform.position;
            targetRot = TransitionPos2Camera.transform.rotation;
        }

        //ThirdPersonCamera.transform.position = Vector3.Lerp(ThirdPersonCamera.transform.position, targetPos, Time.deltaTime * 8.0f);
        //ThirdPersonCamera.transform.rotation = Quaternion.Slerp(ThirdPersonCamera.transform.rotation, targetRot, Time.deltaTime * 8.0f);

        //float magnitudeToTargetPos = (ThirdPersonCamera.transform.position - targetPos).magnitude;


        FlyCamera.transform.position = Vector3.Lerp(FlyCamera.transform.position, targetPos, Time.deltaTime * 8.0f);
        FlyCamera.transform.rotation = Quaternion.Slerp(FlyCamera.transform.rotation, targetRot, Time.deltaTime * 8.0f);

        float magnitudeToTargetPos = (FlyCamera.transform.position - targetPos).magnitude;


        if (magnitudeToTargetPos <= 0.005f)
        {
            HasReachedTransitionPos = true;


        }
    }

    private void ReachToCloseUpPosition()
    {
        Vector3 targetPos = CloseupCamera.transform.position;
        Quaternion targetRot = CloseupCamera.transform.rotation;

        //ThirdPersonCamera.transform.position = Vector3.Lerp(ThirdPersonCamera.transform.position, targetPos, Time.deltaTime * 10.0f);
        //ThirdPersonCamera.transform.rotation = Quaternion.Slerp(ThirdPersonCamera.transform.rotation, targetRot, Time.deltaTime * 10.0f);

        //float magnitudeToTargetPos = (ThirdPersonCamera.transform.position - targetPos).magnitude;

        FlyCamera.transform.position = Vector3.Lerp(FlyCamera.transform.position, targetPos, Time.deltaTime * 10.0f);
        FlyCamera.transform.rotation = Quaternion.Slerp(FlyCamera.transform.rotation, targetRot, Time.deltaTime * 10.0f);

        float magnitudeToTargetPos = (FlyCamera.transform.position - targetPos).magnitude;

        if (magnitudeToTargetPos <= 0.005f)
        {
            IsTakenOverByCheapInterpolations = false;
            RenderAlwaysOnTopCamera.transform.position = CloseupCamera.transform.position;
            FlyCamera.transform.position = CloseupCamera.transform.position;
            FlyCamera.transform.rotation = CloseupCamera.transform.rotation;
            RenderAlwaysOnTopCamera.transform.rotation = CloseupCamera.transform.rotation;
            InventoryViewChanger.CameraReachedTargetPosition();
            //Debug.LogWarning("Reached to target pos");
        }
    }

    private void ReachToTransitionPosition()
    {
        Vector3 targetPos;
        Quaternion targetRot;

        if (IsReachingToTransitionPos1)
        {
            targetPos = TransitionPos1Camera.transform.position;
            targetRot = TransitionPos1Camera.transform.rotation;
        }

        else
        {
            targetPos = TransitionPos2Camera.transform.position;
            targetRot = TransitionPos2Camera.transform.rotation;
        }

        //ThirdPersonCamera.transform.position = Vector3.Lerp(ThirdPersonCamera.transform.position, targetPos, Time.deltaTime * 5.0f);
        //float distanceToTargetPos = (ThirdPersonCamera.transform.position - targetPos).magnitude;

        FlyCamera.transform.position = Vector3.Lerp(FlyCamera.transform.position, targetPos, Time.deltaTime * 5.0f);
        float distanceToTargetPos = (FlyCamera.transform.position - targetPos).magnitude;



        if (distanceToTargetPos <= 1.6f)
        {
            //ThirdPersonCamera.transform.rotation = Quaternion.Slerp(ThirdPersonCamera.transform.rotation, targetRot, Time.deltaTime * 1.6f);
            FlyCamera.transform.rotation = Quaternion.Slerp(FlyCamera.transform.rotation, targetRot, Time.deltaTime * 1.6f);
        }

        if (distanceToTargetPos <= 0.005f)
        {
            HasReachedTransitionPos = true;
            RenderAlwaysOnTopCamera.EnableRenderOnTopCamera();

            //RenderAlwaysOnTopCamera.SetFieldOfView(ThirdPersonCamera.fieldOfView);
            RenderAlwaysOnTopCamera.SetFieldOfView(FlyCamera.fieldOfView);
        }
    }

    #endregion

    #region Scaling
    private void ScaleInTabletObject()
    {
        Vector3 scalerLocal = TabletScaler.transform.localScale;

        TabletScaler.transform.localScale = new Vector3(Mathf.Lerp(scalerLocal.x, OriginalScalerScale.x, Time.deltaTime * incomingScaleSpeed),
                                                        Mathf.Lerp(scalerLocal.y, OriginalScalerScale.y, Time.deltaTime * incomingScaleSpeed),
                                                        Mathf.Lerp(scalerLocal.z, OriginalScalerScale.z, Time.deltaTime * incomingScaleSpeed));
    }

    private void ScaleTabletObjectOut()
    {
        Vector3 scalerLocal = TabletScaler.transform.localScale;

        TabletScaler.transform.localScale = new Vector3(Mathf.Lerp(scalerLocal.x, 0, Time.deltaTime * outgoingScaleSpeed),
                                                        Mathf.Lerp(scalerLocal.y, 0, Time.deltaTime * outgoingScaleSpeed),
                                                        Mathf.Lerp(scalerLocal.z, 0, Time.deltaTime * outgoingScaleSpeed));
    }

    #endregion

    private void SetMapCameraPositionAndRotation()
    {
        MapCamera.transform.position = ThirdPersonCamera.transform.position + Vector3.up * 600;
        MapCamera.transform.rotation = Quaternion.Euler(90, 0, 0);
    }

    private void StopMessingWithCameras()
    {
        GreenBlip.SetActive(false);
        YellowBlip.SetActive(false);


        CinemachineBrain.enabled = true;
        FlyCamera.enabled = false;
        ThirdPersonCamera.enabled = true;

        Graphics.gameObject.SetActive(false);
        IsTakenOverByCheapInterpolations = false;
        MapCamera.gameObject.SetActive(false);


        ThirdPersonController.OnTabletViewChanged(false);

        TabletScaler.transform.localScale = Vector3.zero;

        //Debug.Log("Don't mess with cameras anymore " + Time.time);
        //Debug.Break();
    }


}
