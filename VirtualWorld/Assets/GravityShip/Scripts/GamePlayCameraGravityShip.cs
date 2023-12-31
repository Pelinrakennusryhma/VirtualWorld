using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayCameraGravityShip : MonoBehaviour
{
    // Start is called before the first frame update

    public Camera Camera;
    public Vector3 OriginalCameraPosition;
    public float OriginalCameraFOV;
    public Quaternion OriginalCameraRotation;
    public LevelStartReferenceCamera LevelStartReferenceCamera;

    public float StartTimer = 2.0f;

    public Camera LayeredCamera;

    private void Start()
    {        
        LevelStartReferenceCamera = FindObjectOfType<LevelStartReferenceCamera>(true);
        LevelStartReferenceCamera.gameObject.SetActive(false);

        if (GameManagerGravityShip.Instance.RelaunchingTheSameScene) 
        {
            return;
        }


        Camera = GetComponent<Camera>();
        OriginalCameraPosition = Camera.transform.position;
        OriginalCameraRotation = Camera.transform.rotation;
        OriginalCameraFOV = Camera.fieldOfView;

        Camera.transform.position = LevelStartReferenceCamera.transform.position;
        Camera.transform.rotation = LevelStartReferenceCamera.transform.rotation;
        Camera.fieldOfView = LevelStartReferenceCamera.Camera.fieldOfView;

        if (LayeredCamera != null)
        {
            LayeredCamera.transform.position = Camera.transform.position;
            LayeredCamera.transform.rotation = Camera.transform.rotation;
            LayeredCamera.fieldOfView = Camera.fieldOfView;
        }

        Time.timeScale = 0;
        StartTimer = 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManagerGravityShip.Instance.RelaunchingTheSameScene)
        {
            return;
        }

        StartTimer -= Time.unscaledDeltaTime;

        if (StartTimer <= 0.0f) 
        {
            if (Time.timeScale < 1.0f)
            {

                Time.timeScale += Time.unscaledDeltaTime;

                if (Time.timeScale > 1.0f)
                {
                    Time.timeScale = 1.0f;
                }
            }


            Camera.transform.position = Vector3.Lerp(Camera.transform.position,
                                                     OriginalCameraPosition,
                                                     Time.unscaledDeltaTime * 1.00f);

            float differenceBetweenPositions = (OriginalCameraPosition - Camera.transform.position).magnitude;

            //if (differenceBetweenPositions <= 10.0f) 
            {

                Camera.transform.rotation = Quaternion.Slerp(Camera.transform.rotation,
                                                             OriginalCameraRotation,
                                                             Time.unscaledDeltaTime * 1.00f);
            }
        }

        if (LayeredCamera != null)
        {
            LayeredCamera.transform.position = Camera.transform.position;
            LayeredCamera.transform.rotation = Camera.transform.rotation;
            LayeredCamera.fieldOfView = Camera.fieldOfView;
        }
    }
}
