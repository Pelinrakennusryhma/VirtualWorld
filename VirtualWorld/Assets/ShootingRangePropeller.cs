using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingRangePropeller : MonoBehaviour
{
    private void FixedUpdate()
    {
        transform.Rotate(new Vector3(0, 355.0f * Time.deltaTime, 0), Space.Self);   
    }
}
