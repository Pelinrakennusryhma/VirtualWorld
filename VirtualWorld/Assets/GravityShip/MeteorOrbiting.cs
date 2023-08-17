using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorOrbiting : MonoBehaviour
{
    public GameObject CenterPoint;
    public GameObject OrbitingObject;

    public bool RotateAntiClockWise;
    public float Speed = 300;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        float speed = Speed;
        
        if (RotateAntiClockWise)
        {
            speed = -speed;
        }

        OrbitingObject.transform.RotateAround(CenterPoint.transform.position, 
                                              Vector3.up, 
                                              speed * Time.deltaTime);
    }
}
