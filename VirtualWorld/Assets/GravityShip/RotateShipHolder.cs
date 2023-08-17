using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateShipHolder : MonoBehaviour
{
    public bool RotateClockWise;

    // Update is called once per frame
    void Update()
    {
        float speed = 35.0f;

        if (RotateClockWise)
        {
            transform.Rotate(Vector3.up, speed * Time.deltaTime);
        }

        else
        {
            transform.Rotate(Vector3.up, -speed * Time.deltaTime);
        }
    }
}
