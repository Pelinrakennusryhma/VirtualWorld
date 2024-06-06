using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFallCameraTrigger : MonoBehaviour
{
    public bool ReturnToOriginalOffsetPos;

    public BoatCamera GamePlayCam;

    public Camera WaterFallCamera;



    // Start is called before the first frame update
    void Start()
    {
        WaterFallCamera.enabled = false;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Player triggered");

            if (!ReturnToOriginalOffsetPos)
            {
                GamePlayCam.MoveToWaterFallPosition(WaterFallCamera);
            }

            else
            {
                GamePlayCam.ResumeToNormalCamera();
            }
        }
    }
}
