using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PickUppableObject : NetworkBehaviour
{

    public FirstPersonPlayerController Holder;
    public PickUpFunctionality HolderPickUpFunctionality;

    //public override void OnNetworkSpawn()
    //{
    //    if (!IsOwner)
    //    {
    //        this.enabled = false;
    //    }
    //}

    public virtual void OnPickUp(FirstPersonPlayerController holder,
                                 PickUpFunctionality holderPickUpFunctionality)
    {
        Holder = holder;
        HolderPickUpFunctionality = holderPickUpFunctionality;
        //DespawnOnServerRpc();
        //gameObject.SetActive(false);
        Debug.Log("Picked up " + " " + gameObject.name + " " + Time.time);
    }

    [ServerRpc(RequireOwnership = false)]
    public void DespawnOnServerRpc()
    {
        GetComponentInParent<NetworkObject>().Despawn(false);
        gameObject.SetActive(false);
        Debug.Log("Server rpc called");
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        gameObject.SetActive(false);
    }

    public virtual void OnReleasePickUp()
    {
        Holder = null;
        HolderPickUpFunctionality = null;
        Debug.Log("Released pick up " + " " + gameObject.name + " " + Time.time);
    }


    public virtual void OnReleasePickUp(Vector3 forceToAdd,
                                                 Vector3 angularVelocity)
    {
        Debug.Log("Called base of release pickup client rpc");
        Holder = null;
        HolderPickUpFunctionality = null;
        //Debug.Log("Released pick up " + " " + gameObject.name + " " + Time.time);
    }
}
