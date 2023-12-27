using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingRangeRotatingMovingTarget : ShootingRangeMovingTarget
{
    [SerializeField] private float rotationRate = 5.0f;
    [SerializeField] private bool rotateClockwise = true;


    protected override void FixedUpdate()
    {
        if (!Target.IsDead) 
        {
            base.FixedUpdate();
            
            if (rotateClockwise) 
            {
                ActualMovingObject.transform.Rotate(new Vector3(0, rotationRate * Time.deltaTime, 0));
            }

            else
            {
                ActualMovingObject.transform.Rotate(new Vector3(0, -rotationRate * Time.deltaTime, 0));
            }
        }
    }
}
