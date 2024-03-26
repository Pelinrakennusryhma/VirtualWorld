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

    private List<GameObject> spawnedObjects = new List<GameObject>();

    private GameObject CharacterManager;

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
        int count = 0;

        foreach (GameObject obj in objectsToSpawn)
        {
            GameObject go = Instantiate(obj);

            if (count == 0)
            {
                CharacterManager = go;
            }

            spawnedObjects.Add(go);
            InstanceFinder.ServerManager.Spawn(go);
            count++;
        }


    }

    public void OnEnable()
    {
        //ReInit();
    }

    public void ReInit()
    {
        //for (int i = 0; i < spawnedObjects.Count; i++)
        //{
        //    Destroy(spawnedObjects[i]);
        //}

        //spawnedObjects.Clear();

        //SpawnNetworkObjects();
    }

    public void OnReInitTargetRPC()
    {
        //Destroy(CharacterManager.gameObject);
        //spawnedObjects.Remove(CharacterManager);

        //CharacterManager = Instantiate(objectsToSpawn[0]);
        //spawnedObjects.Add(CharacterManager);
        //InstanceFinder.ServerManager.Spawn(CharacterManager);
        //CharacterManager.GetComponent<Characters.CharacterManager>().Respawn();
    }
}
