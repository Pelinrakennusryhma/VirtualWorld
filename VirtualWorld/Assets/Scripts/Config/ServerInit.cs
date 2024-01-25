using UnityEngine;
using BackendConnection;
using System;
using Scenes;
using Cysharp.Threading.Tasks;
using FishNet;
using FishNet.Managing;
using FishNet.Managing.Scened;
using System.Collections;
using FishNet.Transporting;

namespace Configuration
{
    public class ServerInit : MonoBehaviour
    {
        [SerializeField] APICalls_Client apiCalls_Client;
        [SerializeField] ScenePicker mainScenePicker;
        [SerializeField] NetworkManager networkManager;
        [SerializeField] SceneManager sceneManager;

        [SerializeField] NetworkObjectSpawner networkObjectSpawner;

        [Tooltip("Objects that server doesn't need so they can be destroyed")]
        [SerializeField] GameObject[] objectsToDespawn;
        public async UniTask Init()
        {
            Debug.Log("--- SERVER INIT START ---");
            networkManager.ServerManager.OnServerConnectionState += OnServerStarted;
            networkManager.ServerManager.StartConnection();
        }

        void OnServerStarted(ServerConnectionStateArgs args)
        {
            if(args.ConnectionState == LocalConnectionState.Started)
            {
                DestroyObjectsOnServer();
                networkObjectSpawner.Init();
            }
        }

        void DestroyObjectsOnServer()
        {
            foreach (GameObject go in objectsToDespawn)
            {
                Destroy(go);
            }
        }
    }
}


