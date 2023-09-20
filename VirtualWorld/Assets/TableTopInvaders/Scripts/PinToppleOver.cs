using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinToppleOver : MonoBehaviour
{
    public Pin Pin;

    public void OnCollisionEnter(Collision collision)
    {
        if (!Pin.HasToppledOver)
        {
            Pin.OnCollision(collision);
        }
    }
}
