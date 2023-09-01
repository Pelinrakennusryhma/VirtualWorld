using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class DEBUG_Ball : NetworkBehaviour
{
    void Awake()
    {
        Debug.Log("--------- Playground Scene Loaded! ---------");
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        }
    }

    void OnClientConnected(ulong clientId)
    {
        Debug.Log($"---- CLIENT {clientId} CONNECTED ----");
    }
}
