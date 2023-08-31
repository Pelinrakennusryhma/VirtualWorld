using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkDiceSpawner : NetworkBehaviour
{
    public GameObject DiceOriginal;

    // Awake does not server variables setup yet. So ishost and isserver return false
    public void Start()
    {
        Debug.Log("Ishost is " + IsHost + " isserver " + IsServer);

        if (IsHost || IsServer) 
        {
            GameObject go = Instantiate(DiceOriginal, transform.position, Quaternion.identity);

            NetworkObject[] nos = go.GetComponentsInChildren<NetworkObject>();

            for (int i = 0; i < nos.Length; i++)
            {
                nos[i].Spawn();
                //nos[i].SpawnWithOwnership(NetworkManager.Singleton.LocalClientId, true);
            }

            //go.GetComponent<NetworkObject>().Spawn();
        }
    }
}
