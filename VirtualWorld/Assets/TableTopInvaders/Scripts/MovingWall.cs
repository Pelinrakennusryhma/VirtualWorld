using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingWall : MonoBehaviour
{
    public enum WallMovementType
    {
        None = 0,
        PingPong = 1,
        Sine = 2,
        Curves = 3
    }

    public GameObject Pos1;
    public GameObject Pos2;
    public GameObject ActualMovingObject;

    public WallMovementType MovementType;
    public float StartPos;
    public float Speed;

    public AnimationCurve Curve1;
    public AnimationCurve Curve2;
    private float CurvePos;

    private void Awake()
    {
        Pos1.gameObject.SetActive(false);
        Pos2.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (MovementType)
        {
            case WallMovementType.None:
                break;
            case WallMovementType.PingPong:
                DoPingPong();
                break;
            case WallMovementType.Sine:
                DoSine();
                break;
            case WallMovementType.Curves:
                DoCurves();
                break;
            default:
                break;
        }


    }

    private void DoPingPong()
    {
        float fromZeroToOne = Mathf.PingPong(Time.time * Speed + StartPos, 1.0f);

        ActualMovingObject.transform.position = Vector3.Lerp(Pos1.transform.position,
                                                             Pos2.transform.position,
                                                             fromZeroToOne);
    }

    private void DoSine()
    {
        float fromZeroToOne = (Mathf.Sin(Time.time * Speed + StartPos) + 1.0f) / 2.0f;

        ActualMovingObject.transform.position = Vector3.Lerp(Pos1.transform.position,
                                                             Pos2.transform.position,
                                                             fromZeroToOne);
    }

    private void DoCurves()
    {
        CurvePos += Time.deltaTime;

        if (CurvePos >= 2.0f)
        {
            CurvePos -= (int) CurvePos;
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
