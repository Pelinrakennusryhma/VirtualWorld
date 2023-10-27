using UnityEngine;
using StarterAssets;
using FishNet.Object;
using UnityEngine.Events;
using Characters;

// DOES THIS NEED TO BE A NetworkBehaviour?
// Probably not, other than checking for ownership.



// This component is in charge of moving and changing cameras,
// when tablet view is activated

public class TabletCameraViewController : NetworkBehaviour
{
    // The main camera
    [Tooltip("NetworkPlayer01/MainCamera")]
    [SerializeField] private Camera ThirdPersonCamera;

    // This gameobjects's position and rotation is to reach when the tablet view is activated
    [Tooltip("Child: NetworkPlayer01/Tablet/Virtual Camera Closeup")]
    [SerializeField] private GameObject CloseupCamera;

    // This gameobject is transition pos on right shoulder of the character,
    // that is passed through while reaching to the actual closeup position
    [Tooltip("Child: NetworkPlayer01/Tablet/Virtual Camera Transition Pos1")]
    [SerializeField] private GameObject TransitionPos1Camera;

    // This gameobject is transition pos on left shoulder of the character,
    // that is passed through while reaching to the actual closeup position
    [Tooltip("Child: NetworkPlayer01/Tablet/Virtual Camera Transition Pos2")]
    [SerializeField] private GameObject TransitionPos2Camera;

    // Just a reference to inputs, to know if the tablet button
    // has been pressed and the view is active
    [Tooltip("Component on NetworkPlayer01")]
    [SerializeField] private StarterAssetsInputs Inputs;

    // This is the camera that gets activated as the actual flying camera.
    // The main camera (ThirdPersonCamera) is left as is, because it's under
    // control of Cinemachine brain, and it's easier to just do the view
    // transitions without messing with the main camera
    [Tooltip("Found under ViewWithinAViewObjects/FlyCamera")]
    [SerializeField] private FlyCamera FlyingCam;
    //------------------------------------------------------------------------------------------------------

    // Just a bool that tracks whether or not the tablet view is active
    private bool isActiveTabletView;

    // A bool to keep track if we have reached TransitionPos1Camera's
    // or TransitionPos2Camera's position and rotation
    private bool hasReachedTransitionPos;

    // A bool to keep track which transition pos was chosen
    private bool isReachingToTransitionPos1;

    // Are we reaching to something and interpolating towards a position?
    private bool isInterpolating;

    // The speed at which the tablet lerps in to view
    private float incomingScaleSpeed = 10.0f;

    // The speed at which the tablet lerps out of view
    private float outgoingScaleSpeed = 10.0f;

    // An object that scales the tablet to preferred size when view is activated
    // and to zero when tablet view is inactivated
    [Tooltip("Should be Tablet/Scaler GameObject")]
    [SerializeField] private GameObject TabletMainScaler;

    // Keep track of what was the tablet's original scale before modifying it
    private Vector3 OriginalScalerScale;


    [SerializeField] private ViewWithinAViewController ViewWithinAViewController;


    public override void OnStartClient()
    {
        // Save the tablet scaler object's original scale, because we are about to 
        // set it to zero
        OriginalScalerScale = TabletMainScaler.transform.localScale;

        // We don't ever start with the tablet view active
        // Make sure the bool is false
        isActiveTabletView = false;

        // Just disable objects that we don't need
        TabletMainScaler.gameObject.SetActive(false);

        if (IsOwner)
        {
            ViewWithinAViewController.Init();
            // Subscribe to events about button presses
            PlayerEvents.Instance.EventOpenTabletPressed.AddListener(OnOpenTabletPressed);
            PlayerEvents.Instance.EventCloseTabletPressed.AddListener(OnCloseTabletPressed);
        }

        //Debug.LogWarning("Starting client. Isowner " + IsOwner);

        ViewWithinAViewController.SetupMapBlips(IsOwner,
                                                !IsOwner);

        // Disable the render on top camera, it will be used later
        // but not now    
        // We don't use the FlyCamera yet, so disable it.
        FlyingCam.EnableDisableCameras(false, false);
    }

    #region ButtonPresses

    // Now we know that the tablet button was pressed succesfully
    // Act according to that
    // We either setup needed things to come in or to go out
    public void OnOpenTabletPressed()
    {
        if (!isActiveTabletView)
        {

            SetupTabletForComingIn();
            ViewWithinAViewController.OnTabletOpened();

#if UNITY_WEBGL
            Inputs.UnlockCursor();
#endif
        }
    }

    // callback is called once camera has done zooming back to change StarterAssetsInputs' gameState enum
    public void OnCloseTabletPressed()
    {

        SetupTabletForGoingOut();

    }

    #endregion

    // This region is for setupping the transition beginnings
    // either coming in or going out.

    #region SetupTransitionBeginnings

    private void SetupTabletForComingIn()
    {
        isInterpolating = true;




        // We hide the tablet, because it should not be shown yet.
        TabletMainScaler.transform.localScale = Vector3.zero;

        // Make sure the tablet graphics object is active
        TabletMainScaler.gameObject.SetActive(true);

        // Infrom view within a view controller about camera starting a transition
        ViewWithinAViewController.OnCameraStartedTransitioning();

        // View within a view controller setups the map camera
        // according to the position of ThirdPersonCamera
        ViewWithinAViewController.SetupMapCamera(ThirdPersonCamera.transform.position);

        // This method is only ever called on the owner,
        // so we set the green blip active so we can see
        // ourselves as the green one
        //ViewWithinAViewController.SetupMapBlips(true, false);



        // We haven't yet reached any transition pos
        hasReachedTransitionPos = false;

        // Now the view is active
        isActiveTabletView = true;


        // We disable the main camera that is under control
        // of CinemachineBrain and activate the FlyCamera
        ThirdPersonCamera.enabled = false;
        FlyingCam.EnableDisableCameras(true, false);


        // Set field of view, pos and rot to match
        // the main camera's, so we can start a smooth transition
        FlyingCam.SetCameraValues(ThirdPersonCamera.transform.position,
                                  ThirdPersonCamera.transform.rotation,
                                  ThirdPersonCamera.fieldOfView);


        // Determine which transition position is closer...
        float magnitudeToPos1 = (ThirdPersonCamera.transform.position - TransitionPos1Camera.transform.position).magnitude;
        float magnitudeToPos2 = (ThirdPersonCamera.transform.position - TransitionPos2Camera.transform.position).magnitude;

        // ... and choose the position according to that
        if (magnitudeToPos1 <= magnitudeToPos2)
        {
            isReachingToTransitionPos1 = true;

        }

        else
        {
            isReachingToTransitionPos1 = false;

        }
    }

    private void SetupTabletForGoingOut()
    {
        // We keep track of if we are doing the movements 
        isInterpolating = true;

        // Inform view within a view controller that the camera started transitioning
        ViewWithinAViewController.OnCameraStartedTransitioning();

        // We don't need to render on top anymore,
        // So we disable the render on top camera, just in the case
        // it would render the camera on top of the player.
        // Woulnd't look too good.
        FlyingCam.EnableDisableCameras(true, false);


        // Of course we haven't reached any transition pos yet
        hasReachedTransitionPos = false;

        // We can say, that we aren't on tablet view anymore
        // Even though we are just beginning to transition out.
        isActiveTabletView = false;
    }

    #endregion

    // Update is called once per frame
    void LateUpdate()
    {


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
        if (!isInterpolating)
        {
            return;
        }


        // If the tablet view should be active, reach in
        if (isActiveTabletView)
        {
            // We wan't the actual tablet graphics object
            // scaled in to full size
            ScaleInTabletObject();

            // We haven't reached a shoulder position yet
            if (!hasReachedTransitionPos)
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
            if (!hasReachedTransitionPos)
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
        // How far we are from the target position? float magnitudeToTargetPos        
        // What is the angle between current rotation and target rotation?float angleBetweenRots
        FlyingCam.MoveTowardsTargetPositionAndRotation(targetPos,
                                                       5.0f,
                                                       targetRot,
                                                       5.6f,
                                                       out float magnitudeToTargetPos,
                                                       out float angleBetweenRots);

        // We can scale the tablet out since it shouldn't be seen anymore.
        ScaleTabletObjectOut();

        // If the distance and rotation are close enough
        // we just stop doing the camera things
        // and inform View within a view controller that we are done
        if (magnitudeToTargetPos <= 0.005f
            && angleBetweenRots <= 0.0001f)
        {
            StopMessingWithCameras();

            ViewWithinAViewController.OnTabletClosed();

            // once camera has reached its return position, set gameState to free
            CharacterManager.Instance.SetGameState(GAME_STATE.FREE);
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
        if (isReachingToTransitionPos1)
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
        // How far we are from the target target position? float magnitudeToTargetPos
        FlyingCam.MoveTowardsTargetPositionAndRotation(targetPos,
                                                       8.0f,
                                                       targetRot,
                                                       8.0f,
                                                       out float magnitudeToTargetPos,
                                                       out float angleBetweenRots);

        // If we are close enough, we determine that we have reached the transition position
        if (magnitudeToTargetPos <= 0.005f)
        {
            hasReachedTransitionPos = true;
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
        // How far we are from target? float magnitudeToTargetPos
        FlyingCam.MoveTowardsTargetPositionAndRotation(targetPos,
                                                       10.0f,
                                                       targetRot,
                                                       10.0f,
                                                       out float magnitudeToTargetPos,
                                                       out float angleBetweenRots);

        // If we are close enough, we are finished reaching to closeup position
        // So we set the positions and rotations to those of the targets 
        if (magnitudeToTargetPos <= 0.005f)
        {
            isInterpolating = false;

            //FlyCamera.transform.position = targetPos;
            //FlyCamera.transform.rotation = targetRot;

            FlyingCam.SetCameraPositionAndRotation(targetPos, targetRot);

            // We need to inform inventory view changer on TabletFunctionality that now we are
            // at target position, so it stops rendering to a render texture
            // and switches over to overlay camera and does whatever needs to be done
            // to show the invenotry canvas properly, but perhaps hackishly
            // The render texture screen is visible already, if invenotry was the last screen shown
            // the previous time


            ViewWithinAViewController.OnCameraReachedTransitionPos();
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
        if (isReachingToTransitionPos1)
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
        // How far we are from the target? float distanceToTargetPos
        FlyingCam.MoveTowardsTargetPosition(targetPos, 5.0f, out float distanceToTargetPos);

        // ...if we are close enough: start modifying the rotation
        if (distanceToTargetPos <= 1.6f)
        {
            FlyingCam.MoveTowardsTargetRotation(targetRot, 1.6f, out float angleBetweenRots);
        }

        // If we are even closer, we have reached
        // and should enable the render always on top camera
        if (distanceToTargetPos <= 0.005f)
        {
            hasReachedTransitionPos = true;

            FlyingCam.EnableDisableCameras(true, true);
        }
    }

    #endregion

    #region Scaling

    // Just lerping the scale towards tablet graphic object's original default scale
    private void ScaleInTabletObject()
    {
        Vector3 scalerLocal = TabletMainScaler.transform.localScale;

        TabletMainScaler.transform.localScale = new Vector3(Mathf.Lerp(scalerLocal.x, OriginalScalerScale.x, Time.deltaTime * incomingScaleSpeed),
                                                            Mathf.Lerp(scalerLocal.y, OriginalScalerScale.y, Time.deltaTime * incomingScaleSpeed),
                                                            Mathf.Lerp(scalerLocal.z, OriginalScalerScale.z, Time.deltaTime * incomingScaleSpeed));
    }

    // Lerping tablet graphics object's scale towards zero to hide it.
    private void ScaleTabletObjectOut()
    {
        Vector3 scalerLocal = TabletMainScaler.transform.localScale;

        TabletMainScaler.transform.localScale = new Vector3(Mathf.Lerp(scalerLocal.x, 0, Time.deltaTime * outgoingScaleSpeed),
                                                            Mathf.Lerp(scalerLocal.y, 0, Time.deltaTime * outgoingScaleSpeed),
                                                            Mathf.Lerp(scalerLocal.z, 0, Time.deltaTime * outgoingScaleSpeed));
    }

    #endregion


    // Called when we should return to normal game play view
    private void StopMessingWithCameras()
    {
        //ViewWithinAViewController.SetupMapBlips(false, false);

        // Disable FlyCamera...
        FlyingCam.EnableDisableCameras(false, false);

        // ...because we are switching to normal third person camera
        ThirdPersonCamera.enabled = true;

        // We make sure that the tablet isn't visible anymore
        TabletMainScaler.gameObject.SetActive(false);

        // We are not doing any transitions anymore
        isInterpolating = false;
    }
}
