using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using BackendConnection;
using Cysharp.Threading.Tasks;

namespace Configuration
{
    public class ClientInit : MonoBehaviour
    {
        [SerializeField] APICalls apiCalls;
        [SerializeField] GameObject connectCanvas;

        public void Init(InitData data)
        {
            Debug.Log("--- CLIENT INIT START ---");
            apiCalls.OnAuthSuccess.AddListener(EnableConnectCanvas);
            apiCalls.OnLogout.AddListener(DisableConnectCanvas);

            var utp = (UnityTransport)NetworkManager.Singleton.NetworkConfig.NetworkTransport;
            utp.SetConnectionData(data.ip, data.serverPort);

            if(data.processType == ProcessType.DEV_CLIENT)
            {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                AutoLog(data.username, data.password);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            }
            Debug.Log("--- CLIENT INIT END ---");
        }

        async UniTask AutoLog(string username, string password)
        {
            //apiCalls.LogOut();
            await apiCalls.OnBeginLogin(username, password, false);
            bool clientStarted = NetworkManager.Singleton.StartClient();
            Debug.Log("clientStarted: " + clientStarted);
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
            bool clientStarted = NetworkManager.Singleton.StartClient();
            Debug.Log("Client started by clicking connect button: " + clientStarted);
        }
    }
}
