using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingRangeCircularMovingTarget : ShootingRangeMovingTarget
{
    // Don't mind the things in editor that came from inherited from class.
    // Those are not currently used for anything, but we want most of the
    // functionality from the base class, so we inherit from it

    [SerializeField] private float xzMoveRadius = 5.0f;
    [SerializeField] private float yzMoveRadius = 5.0f;
    [SerializeField] private bool moveXZ = true;
    [SerializeField] private bool moveYZ = false;
    [SerializeField] private bool clockwiseXZ;
    [SerializeField] private bool clockwiseYZ;
    [SerializeField] private float xzSpeed;
    [SerializeField] private float yzSpeed;

    private Vector3 originalLocalPosition;

    protected override void Awake()
    {
        base.Awake();
        ActualMovingObject.gameObject.SetActive(true);

        originalLocalPosition = ActualMovingObject.transform.localPosition;

        Target.SetForceMultiplier(3.0f);
    }

    protected override void FixedUpdate()
    {
        if (Target.IsDead)
        {
            return;
        }

        float xzMoveSpeed = xzSpeed;
        float yzMoveSpeed = yzSpeed;


        if (clockwiseXZ)
        {
            xzMoveSpeed = -xzSpeed;
        }

        if (clockwiseYZ)
        {
            yzMoveSpeed = -yzSpeed;
        }

        Vector3 xzOffset = Vector3.zero;

        if (moveXZ) 
        {
            xzOffset = new Vector3(Mathf.Cos(Time.time * xzMoveSpeed) * xzMoveRadius,
                                   0,
                                   (Mathf.Sin(Time.time * xzMoveSpeed) * xzMoveRadius));
        }

        Vector3 yzOffset = Vector3.zero;

        if (moveYZ)
        {
            yzOffset = new Vector3(0,
                                   Mathf.Cos(Time.time * yzMoveSpeed) * yzMoveRadius,
                                   (Mathf.Sin(Time.time * yzMoveSpeed) * yzMoveRadius));
        }

        ActualMovingObject.transform.localPosition = originalLocalPosition + xzOffset + yzOffset;
    }
}
