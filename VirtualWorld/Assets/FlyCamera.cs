using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyCamera : MonoBehaviour
{
    [SerializeField] private Camera FlyCam;
    [SerializeField] private Camera RenderAlwaysOnTopCam;

    // Sets the camera positions, rotations and field of view
    public void SetCameraValues(Vector3 position,
                                Quaternion rotation,
                                float fieldOfView)
    {
        FlyCam.transform.position = position;
        FlyCam.transform.rotation = rotation;
        FlyCam.fieldOfView = fieldOfView;

        RenderAlwaysOnTopCam.transform.position = position;
        RenderAlwaysOnTopCam.transform.rotation = rotation;
        RenderAlwaysOnTopCam.fieldOfView = fieldOfView;
    }

    // Sets only camera positions and rotations
    public void SetCameraPositionAndRotation(Vector3 position,
                                             Quaternion rotation)
    {
        FlyCam.transform.position = position;
        FlyCam.transform.rotation = rotation;

        RenderAlwaysOnTopCam.transform.position = position;
        RenderAlwaysOnTopCam.transform.rotation = rotation;

    }

    // Enables or disables a camera according to the bools passed in
    public void EnableDisableCameras(bool enableFlyCam,
                                     bool enableRenderOnTopCam)
    {
        // Make sure the gameOject is active/inactive...
        FlyCam.gameObject.SetActive(enableFlyCam);
        // ...and the camera component enabled/disabled
        FlyCam.enabled = enableFlyCam;

        // Make sure the gameOject is active/inactive...
        RenderAlwaysOnTopCam.gameObject.SetActive(enableRenderOnTopCam);
        // ...and the camera component enabled/disabled
        RenderAlwaysOnTopCam.enabled = enableRenderOnTopCam;
    }


    // Combines movement towards position and rotation
    public void MoveTowardsTargetPositionAndRotation(Vector3 targetPos,
                                                     float moveSpeed,
                                                     Quaternion targetRot,
                                                     float rotSpeed,
                                                     out float magnitudeToTargetPos,
                                                     out float angleBetweenRotAndTargetRot)
    {
        MoveTowardsTargetPosition(targetPos, moveSpeed, out magnitudeToTargetPos);
        MoveTowardsTargetRotation(targetRot, moveSpeed, out angleBetweenRotAndTargetRot);
    }

    // Moves the camera towards a target position with a little bit of lerping
    public void MoveTowardsTargetPosition(Vector3 targetPos,
                                          float moveSpeed,
                                          out float magnitudeToTargetPos)
    {
        // Move the camera towards position
        transform.position = Vector3.Lerp(transform.position,
                                          targetPos,
                                          moveSpeed * Time.deltaTime);

        // Measure the magnitude to target position, so we can pass it out
        magnitudeToTargetPos = (transform.position - targetPos).magnitude;

        // Make sure the position of the render always on top camera matches the 
        // main camera's position
        RenderAlwaysOnTopCam.transform.position = transform.position;
    }

    // Moves the camera towards a target rotation with a little bit of slerping
    public void MoveTowardsTargetRotation(Quaternion targetRot,
                                          float rotSpeed,
                                          out float angleBetweenRotAndTargetRot)
    {
        // Move the camera's rotation towards target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation,
                                              targetRot,
                                              rotSpeed * Time.deltaTime);

        // Measure the angle between rotations, so we can return the value as an out parameter
        angleBetweenRotAndTargetRot = Quaternion.Angle(transform.rotation, targetRot);

        // Make sure the rotation of the render always on top camera matches the 
        // main camera's rotation
        RenderAlwaysOnTopCam.transform.rotation = transform.rotation;
    }
}

