using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpTrigger : MonoBehaviour
{
    public PickUpFunctionality PickUpFunctionality;

    private void OnTriggerEnter(Collider other)
    {
        PickUpFunctionality.OnEnterTriggerArea(other.GetComponentInChildren<PickUppableObject>(true));
        //Debug.Log("OnTrigger Enter " + Time.time);
        
    }

    private void OnTriggerExit(Collider other)
    {
        PickUpFunctionality.OnExitTriggerArea(other.GetComponentInChildren<PickUppableObject>(true));
        //Debug.Log("OnTrigger Exit " + Time.time);
    }
}
