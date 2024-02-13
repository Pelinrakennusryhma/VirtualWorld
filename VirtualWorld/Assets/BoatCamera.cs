using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatCamera : MonoBehaviour
{
    public GameObject CameraOffsetPosObject;
    public GameObject PlayerGraphicsObject;


    private float LevelStartTime;


    public bool DoNormalCameraThings;

    public Camera Camera;

    public float originalFieldOView;

    private Camera WaterFallCamera;

    void Start()
    {
        LevelStartTime = Time.time;
        originalFieldOView = Camera.fieldOfView;
        ResumeToNormalCamera();
    }


    void LateUpdate()
    {
        if (DoNormalCameraThings) 
        {
            Camera.fieldOfView = originalFieldOView;

            if (Time.time >= LevelStartTime + 2.0f)
            {
                transform.position = Vector3.Lerp(transform.position, CameraOffsetPosObject.transform.position, 2.0f * Time.deltaTime);
                transform.rotation = Quaternion.Slerp(transform.rotation,
                                                      (Quaternion.LookRotation((PlayerGraphicsObject.transform.position
                                                                                + PlayerGraphicsObject.transform.forward * 2.0f)
                                                                               - transform.position,
                                                                               Vector3.up)),
                                                      3.0f * Time.deltaTime);
            }
        }

        else
        {
            transform.position = Vector3.Lerp(transform.position,
                                              WaterFallCamera.transform.position,
                                              10.0f * Time.deltaTime);

            //transform.rotation = Quaternion.Slerp(transform.rotation,
            //                                      WaterFallCamera.transform.rotation,
            //                                      4.0f * Time.deltaTime);

            transform.rotation = Quaternion.Slerp(transform.rotation,
                                      (Quaternion.LookRotation((PlayerGraphicsObject.transform.position
                                                                + PlayerGraphicsObject.transform.forward * 2.0f)
                                                               - transform.position,
                                                               Vector3.up)),
                                      3.0f * Time.deltaTime);
        }
    }

    public void MoveToWaterFallPosition(Camera cameraToMatch)
    {
        DoNormalCameraThings = false;
        WaterFallCamera = cameraToMatch;


        //Camera.transform.position = WaterFallCamera.transform.position;
        Camera.transform.rotation = WaterFallCamera.transform.rotation;
        Camera.fieldOfView = WaterFallCamera.fieldOfView;
    }

    public void ResumeToNormalCamera()
    {
        DoNormalCameraThings = true;
    }
}
