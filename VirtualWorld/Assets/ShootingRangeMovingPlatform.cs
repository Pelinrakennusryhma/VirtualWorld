using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingRangeMovingPlatform : MonoBehaviour
{
    public GameObject ParentObject;

    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("On collision enter called");

        FirstPersonPlayerControllerShooting controller = collision.gameObject.GetComponent<FirstPersonPlayerControllerShooting>();

        if (controller != null)
        {
            controller.ParentToMovingPlatform(ParentObject.transform);
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        Debug.Log("On collision exit called");

        FirstPersonPlayerControllerShooting controller = collision.gameObject.GetComponent<FirstPersonPlayerControllerShooting>();

        if (controller != null)
        {
            controller.UnparentFromMovingPlatform(ParentObject.transform);
        }
    }
}
