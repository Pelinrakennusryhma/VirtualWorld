using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorOrbitingUneven : MonoBehaviour
{
    public GameObject CenterPoint;
    public GameObject OrbitingObject;
    public Rigidbody Rigidbody;

    //public Vector3 StartVelo;
    //public float Gravity;

    public Vector3 RotateAxis = Vector3.up;
    public float Speed = 100;
    public bool RotateAntiClockWise;

    private Vector3 PositionVector3D;
    private Quaternion WithoutRotation;

    private float Distance;

    public void Start()
    {
        //Rigidbody.AddForce(StartVelo, ForceMode.Impulse);
        RotateAxis = RotateAxis.normalized;
        Distance = (OrbitingObject.transform.position - CenterPoint.transform.position).magnitude;
        //WithoutRotation = OrbitingObject.transform.rotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float speed = Speed;

        if (RotateAntiClockWise)
        {
            speed = -speed;
        }

        OrbitingObject.transform.position = PositionVector3D;

        OrbitingObject.transform.position = CenterPoint.transform.position;
        OrbitingObject.transform.Rotate(RotateAxis, speed * Time.deltaTime);
        OrbitingObject.transform.position = CenterPoint.transform.position + -OrbitingObject.transform.forward * Distance;
       
        //OrbitingObject.transform.RotateAround(CenterPoint.transform.position,
        //                                      RotateAxis,
        //                                      speed * Time.deltaTime);


        PositionVector3D = OrbitingObject.transform.position;
        OrbitingObject.transform.position = new Vector3(OrbitingObject.transform.position.x,
                                                        0,
                                                        OrbitingObject.transform.position.z);
        //OrbitingObject.transform.rotation = WithoutRotation;

    }


    //Vector3 toGravityArea = CenterPoint.gameObject.transform.position - Rigidbody.transform.position;

    //float distance = toGravityArea.magnitude;


    //float ratio = (distance / Gravity);
    //Mathf.Clamp(ratio, 0, 1.0f);
    //Vector3 towardsPlanet = (CenterPoint.gameObject.transform.position - transform.position).normalized;
    ////Vector3 gravityRot = towardsPlanet + Vector3.right * towardsPlanet.magnitude;


    ////Rigidbody.velocity = VeloWithoutGravity;
    //Rigidbody.AddForce(towardsPlanet * 300.0f * ratio);


    ////float ratio = (distance / GravityAreas[i].Gravity);
    ////Mathf.Clamp(ratio, 0, 1.0f);
    ////Vector3 towardsPlanet = (GravityAreas[i].gameObject.transform.position - transform.position);

    //////Vector3 gravityRot = towardsPlanet + Vector3.right * towardsPlanet.magnitude;
    ////Rigidbody.AddForce(towardsPlanet.normalized * Mathf.Pow(distance, 2) * 0.5f *  GravityAreas[i].Gravity);


    //float veloMagClamped = Mathf.Clamp(Rigidbody.velocity.magnitude, 0.0f, 80.0f);


    //Rigidbody.velocity = Rigidbody.velocity.normalized * veloMagClamped;

    ////Debug.DrawRay(Rigidbody.transform.position, VeloWithoutGravity * 10.0f);

}
