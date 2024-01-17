using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingRangeMovingTarget : MovingWall
{
    public TargetPracticeTarget Target;
    private bool targetIsDead;

    protected override void Awake()
    {
        base.Awake();
        Target.OnDeathHappened -= OnTargetDeath;
        Target.OnDeathHappened += OnTargetDeath;
    }

    protected virtual void Start()
    {
        Target.SetKinematic();
        //Target.RegisterToTakeForceOnHit();
    }

    protected override void FixedUpdate()
    {
        if (!Target.IsDead) 
        {
            base.FixedUpdate();
        }
        // Otherwise donät move it anymore and let it
        // be under the control of physics
    }

    protected void OnDestroy()
    {
        Target.OnDeathHappened -= OnTargetDeath;
    }

    public void OnTargetDeath()
    {
    }

    // We don't probably want to mess with the original
    // method in the class we inherited from, because
    // that script is used in old TabletopInvaders
    // and if something goes wrong, it would not be that
    // cool, because the game has been balanced already with
    // the old functionality in mind.
    protected override void DoCurves()
    {
        CurvePos += Time.deltaTime * Speed;

        if (CurvePos >= 2.0f)
        {
            CurvePos -= (int)CurvePos;
        }

        float fromZeroToTwo = CurvePos;

        if (fromZeroToTwo <= 1.0f)
        {
            float valueAtCurve = Curve1.Evaluate(fromZeroToTwo);

            ActualMovingObject.transform.position = Vector3.Lerp(Pos1.transform.position,
                                                                 Pos2.transform.position,
                                                                 valueAtCurve);
        }

        else
        {
            float valueAtCurve = Curve2.Evaluate(fromZeroToTwo - 1.0f);

            ActualMovingObject.transform.position = Vector3.Lerp(Pos2.transform.position,
                                                                 Pos1.transform.position,
                                                                 valueAtCurve);
        }
    }
}
