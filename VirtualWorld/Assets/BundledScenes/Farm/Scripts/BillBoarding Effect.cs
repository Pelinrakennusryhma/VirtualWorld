using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoardingEffect : MonoBehaviour
{
    private Camera Kamera;


    // Start is called before the first frame update
    void Start()
    {
        Kamera = Camera.main;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.LookAt(Kamera.transform);
    }
}
