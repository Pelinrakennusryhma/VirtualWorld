using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityAreaTrigger : MonoBehaviour
{
    public float Gravity;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerControllerGravityShip.Instance.EnteredGravityTriggerArea(this);
        }
    }
}
