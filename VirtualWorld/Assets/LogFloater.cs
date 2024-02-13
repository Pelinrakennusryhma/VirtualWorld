using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogFloater : MonoBehaviour
{

    private float startPos;
    private float startYpos;
    private float floatSpeed;

    void Awake()
    {
        startYpos = transform.position.y;
        startPos = Random.Range(-1.0f, 1.0f);
        floatSpeed = Random.Range(0.7f, 1.3f);
    }

    void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x, 
                                         startYpos + Mathf.Sin((startPos + Time.time) * floatSpeed) * 0.08f,
                                         transform.position.z);
    }
}
