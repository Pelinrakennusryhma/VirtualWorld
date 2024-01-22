using BackendConnection;
using FishNet;
using FishNet.Observing;
using FishNet.Transporting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkObjectSpawner : MonoBehaviour
{
    public static NetworkObjectSpawner Instance;
    [SerializeField] GameObject[] objectsToSpawn;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Called once FishNet server has started
    public void Init()
    {
        SpawnNetworkObjects();
    }

    void SpawnNetworkObjects()
    {
        foreach (GameObject obj in objectsToSpawn)
        {
            GameObject go = Instantiate(obj);
            InstanceFinder.ServerManager.Spawn(go);
        }
    }
}
