using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using BackendConnection;
using System;
using Scenes;
using Cysharp.Threading.Tasks;

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

            var utp = (UnityTransport)NetworkManager.Singleton.NetworkConfig.NetworkTransport;
            utp.SetConnectionData(data.ip, data.serverPort);

            string mainSceneName = mainScenePicker.GetSceneName();
            Debug.Log(mainSceneName);

            NetworkManager.Singleton.StartServer();
            NetworkManager.Singleton.SceneManager.LoadScene(mainSceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
            Debug.Log("--- SERVER INIT END ---");
        }

    }
}


