using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PickUppableDice :  PickUppableObject
{
    public BoxCollider PhysicsCollider;
    public Rigidbody Rigidbody;


    public override void OnPickUp(FirstPersonPlayerController holder, 
                                  PickUpFunctionality holderPickUpFunctionality)
    {
        base.OnPickUp(holder, holderPickUpFunctionality);        

        PhysicsCollider.enabled = false;
        Rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        Rigidbody.isKinematic = true;
        Rigidbody.useGravity = false;
        Debug.Log("On pickup called");
    }

    [ServerRpc]
    public override void OnReleasePickUpServerRpc()
    {
        base.OnReleasePickUpServerRpc();

        PhysicsCollider.enabled = true;
        Rigidbody.isKinematic = false;
        Rigidbody.useGravity = true;
        Rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    public void LateUpdate()
    {
        bool hasHolder = false;

        if (Holder != null)
        {
            hasHolder = true;        
            MoveToHolderPositionServerRpc(Holder.Camera.transform.position + Holder.Camera.transform.forward * 1.5f, hasHolder);
        }

        //if (Holder != null) 
        //{

        //    Rigidbody.transform.position = Holder.Camera.transform.position + Holder.Camera.transform.forward * 1.5f;
        //    Rigidbody.transform.Rotate(Vector3.right *22.0f * Time.deltaTime + Vector3.up * 18.0f * Time.deltaTime);
        //    //Debug.Log("Should move with player " + Time.time);
        //}

        //else if (Rigidbody.velocity.magnitude <= 0.01f)
        //{
        //    PhysicsCollider.enabled = false;
        //    Rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        //    Rigidbody.isKinematic = true;
        //    Rigidbody.useGravity = false;
        //}
    }

    [ServerRpc]
    public void MoveToHolderPositionServerRpc(Vector3 newPos, bool hasHolder)
    {

        if (hasHolder) 
        {
            Rigidbody.transform.position = newPos;
            Rigidbody.transform.Rotate(Vector3.right * 22.0f * Time.deltaTime + Vector3.up * 18.0f * Time.deltaTime);
        }
    }

    [ServerRpc]
    public override void OnReleasePickUpServerRpc(Vector3 forceToAdd,
                                         Vector3 angluarVelocity)
    {
        Debug.Log("Force to add magnitude is " + forceToAdd.magnitude);

        base.OnReleasePickUpServerRpc(forceToAdd,
                             angluarVelocity);

        PhysicsCollider.enabled = true;
        Rigidbody.isKinematic = false;
        Rigidbody.useGravity = true;
        Rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        Rigidbody.AddForce(forceToAdd, ForceMode.Impulse);
        Rigidbody.angularVelocity = angluarVelocity;
    }
}
