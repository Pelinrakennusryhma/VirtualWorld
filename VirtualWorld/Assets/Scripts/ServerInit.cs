using UnityEngine;
using BackendConnection;
using System;
using Scenes;
using Cysharp.Threading.Tasks;
using FishNet;
using FishNet.Managing;
using FishNet.Managing.Scened;

namespace Configuration
{
    public class ServerInit : MonoBehaviour
    {
        [SerializeField] APICalls apiCalls;
        [SerializeField] ScenePicker mainScenePicker;
        [SerializeField] NetworkManager networkManager;
        [SerializeField] SceneManager sceneManager;
        [SerializeField] GameObject charControlObj;
        public async UniTask Init(InitData data)
        {
            Debug.Log("--- SERVER INIT START ---");
            // Character Controller is not needed on server
            charControlObj.SetActive(false);

            if (data.processType == ProcessType.DEV_SERVER)
            {
                apiCalls.LogOut();
            }

            await apiCalls.OnBeginLogin(data.username, data.password, false);

            networkManager.ServerManager.StartConnection();

            string mainSceneName = mainScenePicker.GetSceneName();
            SceneLoadData sld = new SceneLoadData(mainSceneName);
            sceneManager.LoadGlobalScenes(sld);

            //unload launch scene as it's no longer needed
            sceneManager.UnloadConnectionScenes(new SceneUnloadData(UnityEngine.SceneManagement.SceneManager.GetSceneByBuildIndex(0)));
            Debug.Log("--- SERVER INIT END ---");
        }

    }
}


