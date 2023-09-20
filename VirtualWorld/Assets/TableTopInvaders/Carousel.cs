using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carousel : MonoBehaviour
{
    public bool RotateClockwise;
    private float RotateSpeed = 1.0f;

    public int Level;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Level == 24)
        {
            RotateSpeed = 77;
        }

        else
        {
            RotateSpeed = 33;
        }

        if (RotateClockwise) 
        {

            transform.Rotate(Vector3.up, -RotateSpeed * Time.deltaTime);
        }

        else
        {
            transform.Rotate(Vector3.up, RotateSpeed * Time.deltaTime);
        }
    }
}
