using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetRotation : MonoBehaviour
{
    public Vector3 RotateAxis;



    // Update is called once per frame
    void Update()
    {
        transform.Rotate(RotateAxis, 60.0f * Time.deltaTime);
    }
}
