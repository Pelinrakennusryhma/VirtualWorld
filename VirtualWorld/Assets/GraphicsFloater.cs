using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicsFloater : MonoBehaviour
{
    public float FloatSeverity = 1.0f;
    public float FloatSpeed = 1.0f;
    private float yHeight;
    private float originalYOffset;

    public GameObject Parent;

    public Rigidbody Rigidbody;

    private float unevener;

    public MoveAlongASplineTest Mover;


    private void Awake()
    {
        originalYOffset = transform.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        FloatSpeed = 0.6f;
        FloatSeverity = 0.05f;
        unevener = Mathf.Sin(Time.time * 0.9f);
        //unevener = Mathf.Abs(unevener);
        unevener *= 1.1f;
        float floattySpeedo = FloatSpeed * 0.01f * Rigidbody.velocity.magnitude;

        //yHeight = Mathf.Sin(Time.time * floattySpeedo) * FloatSeverity;

        yHeight = Mathf.Sin(Time.time * floattySpeedo) * FloatSeverity * unevener;

        //transform.position = new Vector3(Parent.transform.position.x,
        //                                 Parent.transform.position.y + yHeight + originalYOffset,
        //                                 Parent.transform.position.z);

        float unevener2 = Mathf.Sin(Time.time * 10.89f) * Mathf.Cos(Time.time * 11.47f);

        //transform.Rotate(transform.right, -yHeight * 1.1f);
        //transform.Rotate(transform.forward, yHeight * unevener2);

 
        Vector3 uppish;

        if (Mover.Inputs.x < -0.1f)
        {
            uppish = new Vector3(0, 0, 4.0f);
        }

        else if (Mover.Inputs.x > 0.1f)
        {
            uppish = new Vector3(0, 0, -4.0f);
        }

        else
        {
            uppish = Vector3.up;
        }

        transform.localRotation = Quaternion.RotateTowards(transform.localRotation,
                                                           Quaternion.Euler(uppish),
                                                           33.9f * Time.deltaTime);

        //Debug.Log("Input is " + Mover.Inputs + " uppish is " + uppish);
    }
}
