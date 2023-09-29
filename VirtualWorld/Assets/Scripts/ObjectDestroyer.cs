using FishNet;
using FishNet.Transporting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroyer : MonoBehaviour
{
    [Header("Destroyed On Server")]
    [SerializeField] GameObject[] serverDestroyedObjects;

    void Awake()
    {
        if (InstanceFinder.NetworkManager.IsServer)
        {
            DestroyObjectsForServer();
        }
    }

    void DestroyObjectsForServer()
    {
        foreach (GameObject gameObject in serverDestroyedObjects)
        {
            Destroy(gameObject);
        }
    }
}
