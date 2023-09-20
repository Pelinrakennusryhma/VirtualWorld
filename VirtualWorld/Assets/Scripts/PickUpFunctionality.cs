using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PickUpFunctionality : NetworkBehaviour
{
    public LayerMask PickUpLayerMask;

    public List<PickUppableObject> PickUppableObjectsInTriggerArea;
    public PickUppableObject CurrentlyHoldingObject;

    public FirstPersonPlayerController FirstPersonPlayerController;

    public float LaunchForce;

    public PickUppableObject PickuppableInHands;

    // Start is called before the first frame update
    void Start()
    {

    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            this.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance != null)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.E)
            && CurrentlyHoldingObject == null)
        {
            PickUppableObject closestObject = null;
            float distanceToClosest = 10000.0f;

            for (int i = 0; i < PickUppableObjectsInTriggerArea.Count; i++)
            {
                if (PickUppableObjectsInTriggerArea[i] == null)
                {
                    //Debug.LogError("Null object. What the hell is happening here?");
                    continue;
                }

                float distance = (transform.position - PickUppableObjectsInTriggerArea[i].transform.position).magnitude;

                if (distance <= distanceToClosest)
                {
                    closestObject = PickUppableObjectsInTriggerArea[i];
                    distanceToClosest = distance;
                }
            }

            if (closestObject != null)
            {
                CurrentlyHoldingObject = closestObject;
                CurrentlyHoldingObject.OnPickUp(FirstPersonPlayerController,
                                                this);
            }

            //Debug.Log("Should pick up " + Time.time);
        }



        if (Input.GetButtonDown("Fire1")
            && CurrentlyHoldingObject != null )
        {
            LaunchObject();
        }
    }


    private void LaunchObject()
    {
        LaunchForce = Random.Range(10.0f, 20.0f);
        //Debug.Log("Launchforce " + LaunchForce);
        //Debug.Log("Release holded object!! " + Time.time);
        Vector3 anglularVelocity = new Vector3(Random.Range(-100, 100),
                                               Random.Range(-100, 100),
                                               Random.Range(-100, 100));

        Debug.Log("About to call client rpc method");
        CurrentlyHoldingObject.OnReleasePickUpServerRpc(FirstPersonPlayerController.Camera.transform.forward * LaunchForce,
                                               anglularVelocity);
        CurrentlyHoldingObject = null;
    }

    public void OnEnterTriggerArea(PickUppableObject pickUppableObject)
    {
        if (!PickUppableObjectsInTriggerArea.Contains(pickUppableObject)) 
        {
            PickUppableObjectsInTriggerArea.Add(pickUppableObject);
            //Debug.Log("Enter trigger area pickuppable object");
        }
    }

    public void OnExitTriggerArea(PickUppableObject pickUppableObject)
    {
        if (PickUppableObjectsInTriggerArea.Contains(pickUppableObject)) 
        {
            PickUppableObjectsInTriggerArea.Remove(pickUppableObject);
            //Debug.Log("Exit trigger area pickuppable object");
        }
    }
}
