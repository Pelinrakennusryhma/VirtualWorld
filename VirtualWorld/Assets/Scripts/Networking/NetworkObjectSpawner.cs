using BackendConnection;
using FishNet;
using FishNet.Transporting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkObjectSpawner : MonoBehaviour
{
    public static NetworkObjectSpawner Instance;
    [SerializeField] GameObject characterManagerPrefab;
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

        InstanceFinder.ServerManager.OnServerConnectionState -= OnServerStarted;
        InstanceFinder.ServerManager.OnServerConnectionState += OnServerStarted;

        //InstanceFinder.ClientManager.OnClientConnectionState -= OnClientStarted;
        //InstanceFinder.ClientManager.OnClientConnectionState += OnClientStarted;

    }

    void OnServerStarted(ServerConnectionStateArgs args)
    {
        if (args.ConnectionState == LocalConnectionState.Started)
        {
            SpawnServerCharacterManager();
        }
    }

    public void SpawnServerCharacterManager()
    {
        GameObject go = Instantiate(characterManagerPrefab);
        InstanceFinder.ServerManager.Spawn(go);
    }

    //void OnClientStarted(ClientConnectionStateArgs args)
    //{
    //    if (args.ConnectionState == LocalConnectionState.Started)
    //    {
    //        SpawnClientCharacterManager();
    //    }
    //}

    //public void SpawnClientCharacterManager()
    //{
    //    Debug.Log("SPAWN CHARACTER MANAGER FOR CLIENT????");
    //}
}
