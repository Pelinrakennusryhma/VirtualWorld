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

namespace Dev
{
    public class DevInit : MonoBehaviour
    {
        [SerializeField] ScenePicker mainScenePicker;
        public void Awake()
        {
            Debug.Log("--- SERVER INIT START ---");
            InstanceFinder.ServerManager.OnServerConnectionState += OnServerStarted;
            InstanceFinder.SceneManager.OnLoadEnd += SceneManager_OnLoadEnd;
        }

        void OnServerStarted(ServerConnectionStateArgs args)
        {
            if (args.ConnectionState == LocalConnectionState.Started)
            {
                LoadMainScene();
            }
        }

        private void SceneManager_OnLoadEnd(SceneLoadEndEventArgs args)
        {
            if (!InstanceFinder.NetworkManager.IsHost && InstanceFinder.NetworkManager.IsClient)
            {
                UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("NetworkLauncher");
            }
        }

        void LoadMainScene()
        {
            string mainSceneName = mainScenePicker.GetSceneName();
            SceneLoadData sld = new SceneLoadData(mainSceneName);
            InstanceFinder.SceneManager.LoadGlobalScenes(sld);

            //unload launch scene as it's no longer needed
            InstanceFinder.SceneManager.UnloadConnectionScenes(new SceneUnloadData("NetworkLauncher"));
            Debug.Log("--- SERVER INIT END ---");
        }
    }
}
