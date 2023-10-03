using UnityEngine;
using StarterAssets;
using FishNet.Object;
using UnityEngine.Events;


// This component is in charge of moving and changing cameras,
// when tablet view is activated

public class TabletCameraViewController : NetworkBehaviour
{   
    // The main camera
    public Camera ThirdPersonCamera; 

    // This gameobjects's position and rotation is to reach when the tablet view is activated
    public GameObject CloseupCamera; 

    // This gameobject is transition pos on right shoulder of the character,
    // that is passed through while reaching to the actual closeup position
    public GameObject TransitionPos1Camera; 

    // This gameobject is transition pos on left shoulder of the character,
    // that is passed through while reaching to the actual closeup position
    public GameObject TransitionPos2Camera; 

    // Just a reference to inputs, to know if the tablet button
    // has been pressed and the view is active
    public StarterAssetsInputs Inputs; 

    // Just a bool that tracks whether or not the tablet view is active
    public bool IsActiveTabletView; 

    // A bool to keep track if we have reached TransitionPos1Camera's
    // or TransitionPos2Camera's position and rotation
    public bool HasReachedTransitionPos; 

    // A bool to keep track which transition pos was chosen
    public bool IsReachingToTransitionPos1;
    
    // Are we reaching to something and interpolating towards a position?
    public bool IsInterpolating; 

    public GameObject PlayerCameraRoot;


    // TO BE REFACTORED ---------------------------------
    public Camera MapCamera;

    public Vector3 MapStartPos;
    public Quaternion MapStartRot;
    public float MapStartFarClipPlane;

    public GameObject GreenBlip;
    public GameObject YellowBlip;
    // --------------------------------------------------

    // This is the camera that render's objects on AlwaysRenderOnTop -layer as a stacked overlay camera
    // This is to prevent a situation where tablet is opened next to a wall for example
    // and the tablet is rendered inside the wall.
    // Children of the FlyCamera
    public RenderAlwaysOnTopCamera RenderAlwaysOnTopCamera; 

    // This is the camera that gets activated as the actual flying camera.
    // The main camera (ThirdPersonCamera) is lef as is, because it's under
    // control of Cinemachine brain, and it's easier to just do the view
    // transitions without messing with the main camera
    public Camera FlyCamera; 

    // Reference to the third person controller, so we can check if we are grounded
    // when the tablet button is pressed
    public ThirdPersonController ThirdPersonController;

    private float incomingScaleSpeed = 10.0f;
    private float outgoingScaleSpeed = 10.0f;

    // An object that scales the tablet to preferred size when view is activated
    // and to zero when tablet view is inactivated
    public GameObject TabletScaler;

    // Keep track of what was the tablet's original scale before modifying it
    private Vector3 OriginalScalerScale;

    // The object that does the actual changing of the
    // views that are shown on the tablet
    // In this component we just inform TabletFunctionality that
    // the view is opened or closed
    private TabletFunctionalityController TabletFunctionality;


    // We inform InventoryViewChanger about if we went over left shoulder
    // This is because for some weird reason the inventory canvas objects
    // positions don't match the tablet screen as an overlay, so the position
    // is adjusted on x-dimension depending over which shoulder we passed through
    public InventoryViewChanger InventoryViewChanger;

    private UnityAction inputsCallback;


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
        // Save the tablet scaler object's original scale, because we are about to 
        // set it to zero
        OriginalScalerScale = TabletScaler.transform.localScale; 
        
        // We don't ever start with the tablet view active
        // Make sure the bool is false
        IsActiveTabletView = false;
        
        // Just disable objects that we don't need
        TabletScaler.gameObject.SetActive(false);







        // TO BE REFACTORED --------------------------------
        MapCamera.gameObject.SetActive(false);



        // According to if we are the owner we either set
        // yellow or green map blip active, so we 
        // see ourselves as the green one and others
        // as yellow on the map
        YellowBlip.gameObject.SetActive(!IsOwner);
        GreenBlip.gameObject.SetActive(IsOwner);
        //-----------------------------------






        // Disable the render on top camera, it will be used later
        // but not now
        RenderAlwaysOnTopCamera.DisableRenderOnTopCamera();

        // We don't use the FlyCamera yet, so disable it.
        FlyCamera.enabled = false;

        Inputs.EventOpenTabletPressed.AddListener(OnOpenTabletPressed);
        Inputs.EventCloseTabletPressed.AddListener(OnCloseTabletPressed);
    }



    // Now we know that the tablet button was pressed succesfully
    // Act according to that
    // We either setup needed things to come in or to go out
    public void OnOpenTabletPressed()
    {
        if (!IsActiveTabletView)
        {
            inputsCallback = null;
            SetupTabletForComingIn();
            TabletFunctionality.OnTabletOpened();
#if UNITY_WEBGL
            Inputs.UnlockCursor();
#endif
        }
    }

    // callback is called once camera has done zooming back to change StarterAssetsInputs' gameState enum
    public void OnCloseTabletPressed(UnityAction callback)
    {
        IsTakenOverByCheapInterpolations = true;

        inputsCallback = callback;
        SetupTabletForGoingOut();
#if UNITY_WEBGL
        Inputs.LockCursor();
#endif
      
    }

    // This region is for setupping the transition beginnings
    // either coming in or going out.

    #region SetupTransitionBeginnings

    private void SetupTabletForComingIn()
    {
        IsInterpolating = true;


        // TO BE REFACTORED? ---------------------
        InventoryViewChanger.CameraStartedTransitioning();
        // --------------------------------------







        // We hide the tablet, because it should not be shown yet.
        TabletScaler.transform.localScale = Vector3.zero;

        // Make sure the tablet graphics object is active
        TabletScaler.gameObject.SetActive(true);










        // TO BE REFACTORED? -----------------------------
        ThirdPersonController.OnTabletViewChanged(true);
        // -----------------------------------------------

        // TO BE REFACTORED? -----------------------------
        // This method is only ever called on the owner,
        // so we set the gree blip active so we can see
        // ourselves as the green one
        YellowBlip.SetActive(false);
        GreenBlip.SetActive(true);

        // We need the map camera. Enable the gameobject
        MapCamera.gameObject.SetActive(true);


        // Save the position and rotation of third person camera,
        // it will be used to determine the map camera position
        MapStartPos = ThirdPersonCamera.transform.position;
        MapStartRot = ThirdPersonCamera.transform.rotation;
        //--------------------------------------------










        // We haven't yet reached any transition pos
        HasReachedTransitionPos = false;

        // Now the view is active
        IsActiveTabletView = true;


        // We disable the main camera that is under control
        // of CinemachineBrain and activate the FlyCamera
        // that is only under the control of this component
        ThirdPersonCamera.enabled = false;
        FlyCamera.enabled = true;

        // Set field of view, pos and rot to match
        // the main camera's, so we can start a smooth transition
        FlyCamera.fieldOfView = ThirdPersonCamera.fieldOfView;
        FlyCamera.transform.position = ThirdPersonCamera.transform.position;
        FlyCamera.transform.rotation = ThirdPersonCamera.transform.rotation;

        // RenderAlwaysOnTopCamera is the children of FlyCamera,
        // so we don't need to mess with positions and rotations
        // field of view is enough
        RenderAlwaysOnTopCamera.SetFieldOfView(ThirdPersonCamera.fieldOfView);


        // Determine which transition position is closer...
        float magnitudeToPos1 = (ThirdPersonCamera.transform.position - TransitionPos1Camera.transform.position).magnitude;
        float magnitudeToPos2 = (ThirdPersonCamera.transform.position - TransitionPos2Camera.transform.position).magnitude;

        // ... and choose the position according to that
        if (magnitudeToPos1 <= magnitudeToPos2)
        {
            IsReachingToTransitionPos1 = true;

        }

        else
        {
            IsReachingToTransitionPos1 = false;

        }




        // TO BE REFACTORED or not? ----------------------------------------

        // We need to inform invenotry about which shoulder we are passing
        // so the canvas objects can be set properly on x-axis
        // An unfortunate hackish solution, but the best I could do.
        InventoryViewChanger.SetWentOverLeftShoulder(!IsReachingToTransitionPos1);
        // -----------------------------------------------------------------




    }

    private void SetupTabletForGoingOut()
    {
        // We keep track of if we are doing the movements 
        IsInterpolating = true;




        // TO Be REFACTORED? ------------------------------------------------
        InventoryViewChanger.CameraStartedTransitioning();
        //------------------------------------------------


        // We don't need to render on top anymore,
        // So we disable the render on top camera, just in the case
        // it would render the camera on top of the player.
        // Woulnd't look too good.
        RenderAlwaysOnTopCamera.DisableRenderOnTopCamera();

        // Of course we haven't reached any transition pos yet
        HasReachedTransitionPos = false;

        // We can say, that we aren't on tablet view anymore
        // Even though we are just beginning to transition out.
        IsActiveTabletView = false;



        // THIS SHOULD BE UNNECESSARY, BECAUSE THE CAMERA HASN'T MOVED.
        // LEAVE IT HERE AS COMMENTED OUT UNTIL SURE. OR IF THINGS CHANGE.
        // THIS WOULD BE THE PLACE TO DETERMINE THE CLOSEST POSITION

        //float magnitudeToPos1 = (ThirdPersonCamera.transform.position - TransitionPos1Camera.transform.position).magnitude;
        //float magnitudeToPos2 = (ThirdPersonCamera.transform.position - TransitionPos2Camera.transform.position).magnitude;

        //if (magnitudeToPos1 <= magnitudeToPos2)
        //{
        //    IsReachingToTransitionPos1 = true;
        //}

        //else
        //{
        //    IsReachingToTransitionPos1 = false;
        //}
    }

    #endregion

    // Update is called once per frame
    void LateUpdate()
    {

        // TO BE REFACTORED WITH MAP VIEW?
        // This could be a MonoBehaviour
        // and the new object a NetworkBehaviour

        // If we don't own the networked object, don't do anything
        if (!IsOwner)
        {
            return;
        }


        //if (Inputs.tablet)
        //{
        //    Inputs.ClearTabletInput();


        //    if (ThirdPersonController.Grounded) 
        //    {
        //        OnTabletPressed();
        //    }
        //}

        // If we are not doing any transitions, just stop now
        if (!IsInterpolating)
        {
            return;
        }






        // TO BE REFACTORED WITH OTHER MAP STUFF? ----------------------------------
        SetMapCameraPositionAndRotation();
        // ----------------------------------------------------




        // If the tablet view should be active, reach in
        if (IsActiveTabletView)
        {
            // We wan't the actual tablet graphics object
            // scaled in to full size
            ScaleInTabletObject();

            // We haven't reached a shoulder position yet
            if (!HasReachedTransitionPos)
            {
                ReachInToTransitionPosition();
            }

            // We passed through shoulder position, so
            // reach to close up position
            // where the tablet is interactable and in full view
            else
            {
                ReachToCloseUpPosition();
            }

        }

        // Tablet view is not active. Reach out
        else
        {
            // Hasn't reached a shoulder position yet...
            if (!HasReachedTransitionPos)
            {
                // ...so go towards transition position
                ReachOutToTransitionPosition();
            }

            // We should go towards normal game play view
            else
            {
                ReachOutToGamePlayView();
            }
        }
    }

    #region ReachingToPositions

    // A method that is called when we are going towards normal game play view
    private void ReachOutToGamePlayView()
    {
        // We are reaching towards normal thirdperson camera's position and rotation
        // It has been left inactive and to live on it's own under the control of CineMachineBrain
        Vector3 targetPos = ThirdPersonCamera.transform.position;
        Quaternion targetRot = ThirdPersonCamera.transform.rotation;

        // Interpolate the FlyCamera's position and rotation towards the target's we just determined
        FlyCamera.transform.position = Vector3.Lerp(FlyCamera.transform.position, targetPos, Time.deltaTime * 5.0f);
        FlyCamera.transform.rotation = Quaternion.Lerp(FlyCamera.transform.rotation, targetRot, Time.deltaTime * 5.6f);

        // How far we are from the target position?
        float magnitudeToTargetPos = (FlyCamera.transform.position - targetPos).magnitude;
        
        // What is the angle between current rotation and target rotation?
        float angleBetweenRots = Quaternion.Angle(FlyCamera.transform.rotation, targetRot);

        // We can scale the tablet out since it shouldn't be seen anymore.
        ScaleTabletObjectOut();

        // If the distance and rotation are close enough
        // we just stop doing the camera things
        // and inform TAbletFunctionality that we are done
        if (magnitudeToTargetPos <= 0.005f
            && angleBetweenRots <= 0.0001f)
        {
            StopMessingWithCameras();
            TabletFunctionality.OnTabletClosed();
            // once camera has reached its return position, invoke the callback function in StarterAssetsInputs to set its gameState back to FREE
            inputsCallback?.Invoke();
        }
    }


    // Method that is called when we are exiting tablet view,
    // but haven't yet reached a shoulder position
    private void ReachOutToTransitionPosition()
    {
        Vector3 targetPos;
        Quaternion targetRot;

        // The thirdperson camera was left to it's own
        // and we pass through the transition position we came in
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

        // We interpolate FlyCamera towards target position and rotation from the current position and rotation
        FlyCamera.transform.position = Vector3.Lerp(FlyCamera.transform.position, targetPos, Time.deltaTime * 8.0f);
        FlyCamera.transform.rotation = Quaternion.Slerp(FlyCamera.transform.rotation, targetRot, Time.deltaTime * 8.0f);

        // How far we are from the target target position
        float magnitudeToTargetPos = (FlyCamera.transform.position - targetPos).magnitude;

        // If we are close enough, we determine that we have reached the transition position
        if (magnitudeToTargetPos <= 0.005f)
        {
            HasReachedTransitionPos = true;


        }
    }

    // This method is called, when we have passed through the transition position
    // and are about to go towards the closeup view of the tablet
    private void ReachToCloseUpPosition()
    {
        // We are reaching towards CloseUpCamera object's position and rotation
        Vector3 targetPos = CloseupCamera.transform.position;
        Quaternion targetRot = CloseupCamera.transform.rotation;

        // Interpolate towards target position and rotation from the current position and rotation
        FlyCamera.transform.position = Vector3.Lerp(FlyCamera.transform.position, targetPos, Time.deltaTime * 10.0f);
        FlyCamera.transform.rotation = Quaternion.Slerp(FlyCamera.transform.rotation, targetRot, Time.deltaTime * 10.0f);

        // How far we are from target?
        float magnitudeToTargetPos = (FlyCamera.transform.position - targetPos).magnitude;

        // If we are close enough, we are finished reaching to closeup position
        // So we set the positions and rotations to those of the targets 
        if (magnitudeToTargetPos <= 0.005f)
        {
            IsInterpolating = false;

            FlyCamera.transform.position = targetPos;
            FlyCamera.transform.rotation = targetRot;           
            


            // TO BE REFACTORED?------------------------------------
            // We need to inform inventory view changer that now we are
            // at target position, so it stops rendering to a render texture
            // and switches over to overlay camera and does whatever needs to be done
            // to show the invenotry canvas properly, but perhaps hackishly
            // The render texture screen is visible already, if invenotry was the last screen shown
            // the previous time
            InventoryViewChanger.CameraReachedTargetPosition();
            //-------------------------------------------------------

        }
    }

    // Method that is called, when we have opened the tablet, but we haven't
    // yet reached a should transition position
    private void ReachInToTransitionPosition()
    {
        Vector3 targetPos;
        Quaternion targetRot;

        // We need to determine the target position and rotation
        // by whichever was closest
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

        // Interpolate FlyCamera's position towards the target position
        FlyCamera.transform.position = Vector3.Lerp(FlyCamera.transform.position, targetPos, Time.deltaTime * 5.0f);
        
        // How far we are from the target?
        float distanceToTargetPos = (FlyCamera.transform.position - targetPos).magnitude;

        // ...if we are close enough: start modifying the rotation
        if (distanceToTargetPos <= 1.6f)
        {
            FlyCamera.transform.rotation = Quaternion.Slerp(FlyCamera.transform.rotation, targetRot, Time.deltaTime * 1.6f);
        }

        // If we are even closer, we have reached
        // and should enable the render always on top camera
        if (distanceToTargetPos <= 0.005f)
        {
            HasReachedTransitionPos = true;
            RenderAlwaysOnTopCamera.EnableRenderOnTopCamera();

            // Make sure the field of view matches FlyCamera's
            RenderAlwaysOnTopCamera.SetFieldOfView(FlyCamera.fieldOfView);
        }
    }

    #endregion

    #region Scaling

    // Just lerping the scale towards tablet graphic object's original default scale
    private void ScaleInTabletObject()
    {
        Vector3 scalerLocal = TabletScaler.transform.localScale;

        TabletScaler.transform.localScale = new Vector3(Mathf.Lerp(scalerLocal.x, OriginalScalerScale.x, Time.deltaTime * incomingScaleSpeed),
                                                        Mathf.Lerp(scalerLocal.y, OriginalScalerScale.y, Time.deltaTime * incomingScaleSpeed),
                                                        Mathf.Lerp(scalerLocal.z, OriginalScalerScale.z, Time.deltaTime * incomingScaleSpeed));
    }

    // Lerping tablet graphics object's scale towards zero to hide it.
    private void ScaleTabletObjectOut()
    {
        Vector3 scalerLocal = TabletScaler.transform.localScale;

        TabletScaler.transform.localScale = new Vector3(Mathf.Lerp(scalerLocal.x, 0, Time.deltaTime * outgoingScaleSpeed),
                                                        Mathf.Lerp(scalerLocal.y, 0, Time.deltaTime * outgoingScaleSpeed),
                                                        Mathf.Lerp(scalerLocal.z, 0, Time.deltaTime * outgoingScaleSpeed));
    }

    #endregion

    // TO BE REFACTORED? with blips and stuff? -------------------------------
    private void SetMapCameraPositionAndRotation()
    {
        // Map camera should be from above the player facing down, so we make it so.
        MapCamera.transform.position = ThirdPersonCamera.transform.position + Vector3.up * 600;
        MapCamera.transform.rotation = Quaternion.Euler(90, 0, 0);
    }
    //--------------------------------------------------

    // Called when we should return to normal game play view
    private void StopMessingWithCameras()
    {
        GreenBlip.SetActive(false); // WHY?
        YellowBlip.SetActive(false); // WHY?


        // Disable FlyCamera...
        FlyCamera.enabled = false;
        // ...because we are switching to normal third person camera
        ThirdPersonCamera.enabled = true;

        // We make sure that the tablet isn't visible anymore
        TabletScaler.gameObject.SetActive(false);

        // We are not doing any transitions anymore
        IsInterpolating = false;


        // TO BE REFACTORED? ------------------------------
        // We don't need to render the map camera anymore
        MapCamera.gameObject.SetActive(false);
        //-------------------------------------------------




        // TO BE REFACTORED? ---------------------------------------
        // Inform third person controller that tablet view is not active anymore.
        ThirdPersonController.OnTabletViewChanged(false);
        //----------------------------------------------------------


    }


}
