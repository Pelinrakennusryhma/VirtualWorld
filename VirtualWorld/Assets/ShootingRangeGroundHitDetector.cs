using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingRangeGroundHitDetector : MonoBehaviour
{
    public Health Health;

    public void OnTriggerEnter(Collider other)
    {
        if (Health.HitPoints >= Health.MaxHitPoints
            && other.gameObject.CompareTag("Ground")) 
        {
            Debug.Log("Triggered from ground, should die. ");


            //Debug.Log("Triggered from ground, should die. The other parent is " 
            //          + other.gameObject.transform.parent.name + " parent is " + transform.parent.gameObject.name);

            Health.TakeDamage(Health.MaxHitPoints);
        }
    }
}
