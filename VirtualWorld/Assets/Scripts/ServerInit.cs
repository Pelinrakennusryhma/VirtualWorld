using UnityEngine;
using BackendConnection;
using System;
using Scenes;
using Cysharp.Threading.Tasks;
using Mirror;
using UnityEngine.SceneManagement;

namespace Configuration
{
    public class ServerInit : MonoBehaviour
    { 
        [SerializeField] APICalls apiCalls;
        [SerializeField] ScenePicker mainScenePicker;
        public async UniTask Init(InitData data)
        {
            Debug.Log("--- SERVER INIT START ---");

            if(data.processType == ProcessType.DEV_SERVER)
            {
                apiCalls.LogOut();
            }

            await apiCalls.OnBeginLogin(data.username, data.password, false);

            string mainSceneName = mainScenePicker.GetSceneName();
            Debug.Log(mainSceneName);

            VWNetworkManager.singleton.StartServer();
            Debug.Log("--- SERVER INIT END ---");
        }

    }
}


