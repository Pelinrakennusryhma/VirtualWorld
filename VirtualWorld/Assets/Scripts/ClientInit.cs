using UnityEngine;
using Mirror;
using BackendConnection;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

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

            if (data.processType == ProcessType.DEV_CLIENT)
            {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                AutoLog(data.username, data.password);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            }
            Debug.Log("--- CLIENT INIT END ---");
        }

        async UniTask AutoLog(string username, string password)
        {
            apiCalls.LogOut();
            await apiCalls.OnBeginLogin(username, password, false);
            VWNetworkManager.singleton.StartClient();
            Debug.Log("client autostarted");
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
            VWNetworkManager.singleton.StartClient();
            Debug.Log("Client started by clicking connect button");
        }
    }
}
