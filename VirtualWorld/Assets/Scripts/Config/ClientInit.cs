using UnityEngine;
using System;
using FishNet;
using BackendConnection;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using FishNet.Managing;
using FishNet.Managing.Scened;
using System.Collections;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Transporting;

namespace Configuration
{
    public class ClientInit : MonoBehaviour
    {
        [SerializeField] APICalls_Client apiCalls_Client;
        [SerializeField] GameObject connectCanvas;
        [SerializeField] NetworkManager networkManager;
        [SerializeField] FishNet.Managing.Scened.SceneManager sceneManager;
        [SerializeField] string username;

        InitData data;

        public void Init(InitData data)
        {
            Debug.Log("--- CLIENT INIT START ---");
            this.data = data;

            apiCalls_Client.OnAuthSuccess.AddListener(EnableConnectCanvas);
            apiCalls_Client.OnLogout.AddListener(DisableConnectCanvas);
            networkManager.SceneManager.OnLoadEnd += SceneManager_OnLoadEnd;

            if (data.processType == ProcessType.DEV_CLIENT)
            {
                string username = this.username != "" ? this.username : Environment.GetEnvironmentVariable("UNITY_CLIENT_USERNAME");
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                AutoLog(username, Environment.GetEnvironmentVariable("UNITY_CLIENT_PASSWORD"));
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            }

            Debug.Log("--- CLIENT INIT END ---");
        }

        async UniTask AutoLog(string username, string password)
        {
            apiCalls_Client.LogOut();
            await apiCalls_Client.OnBeginLogin(username, password, false);
            networkManager.ClientManager.StartConnection();
            Debug.Log("client autologged");
        }

        // unload launch scene when the main scene has been loaded
        private void SceneManager_OnLoadEnd(SceneLoadEndEventArgs args)
        {
            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(0);
        }

        void EnableConnectCanvas(LoggedUserData dummyData)
        {
            connectCanvas.SetActive(true);
        }

        void DisableConnectCanvas()
        {
            connectCanvas.SetActive(false);
        }

        public void ConnectToServer()
        {
            networkManager.ClientManager.StartConnection();
            Debug.Log(InstanceFinder.ClientManager.Connection);
            Debug.Log("Client started by clicking connect button");
        }
    }
}
