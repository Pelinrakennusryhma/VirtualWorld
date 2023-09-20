using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtOneDirection : MonoBehaviour
{
    
    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, -1.0f);
    }
}
