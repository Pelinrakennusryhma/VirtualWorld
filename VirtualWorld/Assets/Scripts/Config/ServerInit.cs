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
                LoadMainScene();
            }
        }

        void LoadMainScene()
        {
            string mainSceneName = mainScenePicker.GetSceneName();
            SceneLoadData sld = new SceneLoadData(mainSceneName);
            sceneManager.LoadGlobalScenes(sld);

            //unload launch scene as it's no longer needed
            sceneManager.UnloadConnectionScenes(new SceneUnloadData("Launch"));
            Debug.Log("--- SERVER INIT END ---");
        }

    }
}


