using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using Unity.Netcode;

public class TabletController : NetworkBehaviour
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

    // Start is called before the first frame update
    void Start()
    {
        OriginalScalerScale = TabletScaler.transform.localScale;

        IsActiveTabletView = false;
        Graphics.gameObject.SetActive(false);
        TransitionPos1Camera.gameObject.SetActive(false);
        CloseupCamera.gameObject.SetActive(false);
        MapCamera.gameObject.SetActive(false);

        if (!IsOwner)
        {
            YellowBlip.gameObject.SetActive(true);
            GreenBlip.gameObject.SetActive(false);
        }

        RenderAlwaysOnTopCamera.DisableRenderOnTopCamera();

        FlyCamera.enabled = false;
    }

    public void OnTabletPressed()
    {
        Debug.Log("Tablet button was pressed " + Time.time);
        Inputs.ClearTabletInput();


        IsTakenOverByCheapInterpolations = true;

        if (!IsActiveTabletView)
        {
            TabletScaler.transform.localScale = Vector3.zero;

            ThirdPersonController.OnTabletViewChanged(true);
            YellowBlip.SetActive(false);
            GreenBlip.SetActive(true);

            MapCamera.gameObject.SetActive(true);

            MapStartPos = MainVirtualCamera.transform.position;
            MapStartRot = MainVirtualCamera.transform.rotation;

            HasReachedTransitionPos = false;

            IsActiveTabletView = true;
            //CinemachineBrain.enabled = false;

            Graphics.gameObject.SetActive(true);

            float magnitudeToPos1 = (MainVirtualCamera.transform.position - TransitionPos1Camera.transform.position).magnitude;
            float magnitudeToPos2 = (MainVirtualCamera.transform.position - TransitionPos2Camera.transform.position).magnitude;

            ThirdPersonCamera.enabled = false;
            FlyCamera.enabled = true;
            FlyCamera.fieldOfView = ThirdPersonCamera.fieldOfView;
            FlyCamera.transform.position = ThirdPersonCamera.transform.position;
            FlyCamera.transform.rotation = ThirdPersonCamera.transform.rotation;

            RenderAlwaysOnTopCamera.SetFieldOfView(ThirdPersonCamera.fieldOfView);

            if (magnitudeToPos1 <= magnitudeToPos2)
            {
                IsReachingToTransitionPos1 = true;
            }

            else
            {
                IsReachingToTransitionPos1 = false;
            }
        }

        else
        {
            RenderAlwaysOnTopCamera.DisableRenderOnTopCamera();
            HasReachedTransitionPos = false;

            IsActiveTabletView = false;
            //CinemachineBrain.enabled = true;

            //Graphics.gameObject.SetActive(false);

            float magnitudeToPos1 = (MainVirtualCamera.transform.position - TransitionPos1Camera.transform.position).magnitude;
            float magnitudeToPos2 = (MainVirtualCamera.transform.position - TransitionPos2Camera.transform.position).magnitude;

            if (magnitudeToPos1 <= magnitudeToPos2)
            {
                IsReachingToTransitionPos1 = true;
            }

            else
            {
                IsReachingToTransitionPos1 = false;
            }
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!IsOwner)
        {
            return;
        }

        //if (WaitingToBreak)
        //{
        //    Debug.Log("Third person pos is " + ThirdPersonCamera.transform.position + " virtual camera pos is " + MainVirtualCamera.transform.position);
        //    Debug.Log("Third person eueler rot is " + ThirdPersonCamera.transform.rotation.eulerAngles + " virtual camera rot is " + MainVirtualCamera.transform.rotation.eulerAngles);

        //    Debug.Break();
        //}

        //if (Input.GetKeyDown(KeyCode.M))
        //{
        //    CinemachineBrain.enabled = false;
        //    MapStartPos = ThirdPersonCamera.transform.position;
        //    MapStartRot = ThirdPersonCamera.transform.rotation;
        //    MapStartFarClipPlane = ThirdPersonCamera.farClipPlane;
        //}

        //else if (Input.GetKeyUp(KeyCode.M))
        //{
        //    CinemachineBrain.enabled = true;
        //    ThirdPersonCamera.transform.position = MapStartPos;
        //    ThirdPersonCamera.transform.rotation = MapStartRot;
        //    ThirdPersonCamera.farClipPlane = MapStartFarClipPlane;
        //}

        //if (Input.GetKey(KeyCode.M))
        //{
        //    ThirdPersonCamera.transform.position = new Vector3(MapStartPos.x, MapStartPos.y + 800, MapStartPos.z);
        //    ThirdPersonCamera.transform.rotation = MapStartRot * Quaternion.Euler(90, 0, 0);
        //    ThirdPersonCamera.farClipPlane = MapStartFarClipPlane + 2000;
        //    //ThirdPersonCamera.transform.rotation = MapVirtualCamera.transform.rotation;
        //    Debug.Log("Pressing M " + Time.time);
        //    return;
        //}

        if (Inputs.tablet)
        {
            OnTabletPressed();
        }

        if (!IsTakenOverByCheapInterpolations)
        {
            return;
        }

        MapCamera.transform.position = ThirdPersonCamera.transform.position + Vector3.up * 600;
        MapCamera.transform.rotation =  Quaternion.Euler(90, 0, 0);


        if (IsActiveTabletView)
        {
            Vector3 scalerLocal = TabletScaler.transform.localScale;

            TabletScaler.transform.localScale = new Vector3(Mathf.Lerp(scalerLocal.x, OriginalScalerScale.x, Time.deltaTime * incomingScaleSpeed),
                                                            Mathf.Lerp(scalerLocal.y, OriginalScalerScale.y, Time.deltaTime * incomingScaleSpeed),
                                                            Mathf.Lerp(scalerLocal.z, OriginalScalerScale.z, Time.deltaTime * incomingScaleSpeed));

            if (!HasReachedTransitionPos)
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
                    targetRot= TransitionPos2Camera.transform.rotation;
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

            else
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
                }
            }

        }

        else
        {
            if (!HasReachedTransitionPos)
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

            else
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


                Vector3 scalerLocal = TabletScaler.transform.localScale;

                TabletScaler.transform.localScale = new Vector3(Mathf.Lerp(scalerLocal.x, 0, Time.deltaTime * outgoingScaleSpeed),
                                                                Mathf.Lerp(scalerLocal.y, 0, Time.deltaTime * outgoingScaleSpeed),
                                                                Mathf.Lerp(scalerLocal.z, 0, Time.deltaTime * outgoingScaleSpeed));


                if (magnitudeToTargetPos <= 0.005f
                    && angleBetweenRots <= 0.0001f)
                {
                    WaitingToBreak = true;
                    //Debug.Break();
                    //Debug.Log("Third person pos is " + ThirdPersonCamera.transform.position + " virtual camera pos is " + MainVirtualCamera.transform.position);
                    //Debug.Log("Third person eueler rot is " + ThirdPersonCamera.transform.rotation.eulerAngles + " virtual camera rot is " + MainVirtualCamera.transform.rotation.eulerAngles);
                    StopMessingWithCameras();

                }

                if (magnitudeToTargetPos <= 0.01f) 
                {
                    //Debug.Break();
                }
            }
        }
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

        Debug.Log("Don't mess with cameras anymore " + Time.time);
        //Debug.Break();
    }
}
