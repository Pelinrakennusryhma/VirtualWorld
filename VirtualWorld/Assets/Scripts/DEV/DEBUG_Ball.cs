using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DEBUG_Ball : NetworkBehaviour
{
    void Awake()
    {
        Debug.Log("--------- Playground Scene Loaded! ---------");
    }

    void OnServerConnect(NetworkConnectionToClient conn)
    {
        Debug.Log($"---- CLIENT {conn.connectionId} CONNECTED ----");
    }

}
