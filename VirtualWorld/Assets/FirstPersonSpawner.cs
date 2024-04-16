using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Characters;
using FishNet.Object;

public class FirstPersonSpawner : NetworkBehaviour
{
    public GameObject FPSPrefab;
    public GameObject SpawnedObject;

    public override void OnStartClient()
    {
        if (IsOwner)
        {
            base.OnStartClient();
            CharacterManager.Instance.SetFirstPersonSpawner(this);
        }

    }

    public void OnSpawnFirsPersonController()
    {
        if (SpawnedObject == null) 
        {
            Debug.LogError("Spawning first person controller");

            SpawnedObject = Instantiate(FPSPrefab, transform);
        }
        
    }
}
